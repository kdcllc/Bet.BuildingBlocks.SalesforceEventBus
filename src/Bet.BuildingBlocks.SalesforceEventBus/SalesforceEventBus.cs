using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Salesforce;
using CometD.NetCore.Salesforce.ForceClient;
using Bet.BuildingBlocks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Bet.BuildingBlocks.SalesforceEventBus
{
    /// <summary>
    /// The <see cref="SalesforceEventBus"/> provides a way to register events for salesforce CometD communication bus.
    /// </summary>
    public class SalesforceEventBus : IEventBus
    {
        private readonly IStreamingClient _streamingClient;
        private readonly ILogger<SalesforceEventBus> _logger;
        private readonly IForceClientProxy _forceClient;
        private readonly SalesforceConfiguration _config;
        private readonly IEnumerable<IMessageListener> _messageListerners;

        private readonly ConcurrentDictionary<SubscriptionInfo, object> _subscriptions =
            new ConcurrentDictionary<SubscriptionInfo, object>();

        /// <summary>
        /// Constructor <see cref="SalesforceEventBus"/>
        /// </summary>
        /// <param name="streamingClient">The instance of <see cref="IStreamingClient"/> with connection to salesforce.</param>
        /// <param name="logger">The instance of <see cref="ILogger{SalesforceEventBus}"/>.</param>
        /// <param name="forceClient">The instance of <see cref="ForceClientProxy"/> to provide a publish functionality to the bus.</param>
        /// <param name="messageListeners"></param>
        /// <param name="configuration"></param>
        public SalesforceEventBus(IStreamingClient streamingClient,
            ILogger<SalesforceEventBus> logger,
            IForceClientProxy forceClient,
            IEnumerable<IMessageListener> messageListeners,
            SalesforceConfiguration configuration)
        {
            #region ArgumentNullException
            _streamingClient = streamingClient ?? throw new ArgumentNullException(nameof(streamingClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _forceClient = forceClient ?? throw new ArgumentNullException(nameof(forceClient));
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            #endregion

            _messageListerners = messageListeners;

            _streamingClient.Reconnect += _streamingClient_Reconnect;

            _streamingClient.Handshake();
        }

        private void _streamingClient_Reconnect(object sender, bool isReconnected)
        {
            // possible to add logic to count x times reconnect and stop, at this time
            // the retry will go indefinitely.
            if (isReconnected)
            {
                _streamingClient.Handshake();

                foreach (var sub in _subscriptions)
                {
                    var topicName = GetEventOrTopicName(sub.Key.Name);

                    var messageListener = sub.Value as IMessageListener;

                    _streamingClient.SubscribeTopic(topicName, messageListener, sub.Key.ReplayId);
                }
            }

        }

        ///<inheritdoc/>
        public Task Subscribe<T>(BusEvent eventMessage) where T : class
        {
            #region Dispose and ArgumentException
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Cannot subscribe when disposed");
            }
            if (eventMessage == null)
            {
                throw new ArgumentNullException(nameof(eventMessage));
            }

            var @event = (eventMessage is PlatformEvent) ?
                eventMessage as PlatformEvent :
                throw new ArgumentException($"{nameof(eventMessage)} must be type of {nameof(PlatformEvent)}");

            if (!typeof(IMessageListener).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException("Type parameter must be an IMessageListener");
            }

            var eventName = eventMessage.Name;
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException($"EventName of {nameof(eventMessage)} cannot be empty");
            }
            #endregion

            var key = new SubscriptionInfo(eventName, @event.ReplayId, typeof(T));
            if (_subscriptions.ContainsKey(key))
            {
                // only allow a single subscription per (event + type)
                throw new ArgumentException(
                    $"EventName of {nameof(eventMessage)} is already subscribed to {typeof(T).Name}.");
            }

            // check connection
            if (!_streamingClient.IsConnected)
            {
                _streamingClient.Handshake();
            }

            // build channel segment
            var topicName = GetEventOrTopicName(eventName);
            var handler = GetListerner<T>();

            _subscriptions.AddOrUpdate(key, handler, (existingKey, existingHandler) => existingHandler);

            _streamingClient.SubscribeTopic(topicName, handler as IMessageListener,@event.ReplayId);

            _logger.LogDebug($"{topicName} is subscribed with ReplayId: {@event.ReplayId}");

            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public Task Unsubscribe<T>(BusEvent eventMessage) where T : class
        {
            #region Dispose and ArgumentNullException
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Cannot unsubscribe when disposed");
            }
            if (eventMessage == null)
            {
                throw new ArgumentNullException(nameof(eventMessage));
            }

            var @event = (eventMessage is PlatformEvent) ?
              eventMessage as PlatformEvent :
              throw new ArgumentException($"{nameof(eventMessage)} must be type of {nameof(PlatformEvent)}");

            if (!typeof(IMessageListener).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException("Type parameter must be an IMessageListener");
            }

            var eventName = eventMessage.Name;
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException($"EventName of {nameof(eventMessage)} cannot be empty");
            }
            #endregion

            var key = new SubscriptionInfo(eventName, @event.ReplayId, typeof(T));
            if (!_subscriptions.ContainsKey(key))
            {
                return Task.CompletedTask;
            }

            // build channel segment
            var topicName = GetEventOrTopicName(eventMessage.Name);

            var handler = _subscriptions[key];

            _streamingClient.UnsubscribeTopic(topicName, handler as IMessageListener, @event.ReplayId);

            _subscriptions.TryRemove(key, out var val);

            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public Task Publish(BusEvent eventMessage)
        {
            #region Dispose and ArgumentException
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Cannot unsubscribe when disposed");
            }
            if (eventMessage == null)
            {
                throw new ArgumentNullException(nameof(eventMessage));
            }
            if (string.IsNullOrWhiteSpace(eventMessage.Name))
            {
                throw new ArgumentException(nameof(eventMessage));
            }
            #endregion

            // publish event back to salesforce
            return _forceClient.CreateRecord(eventMessage.Name, eventMessage);
        }

        ///<inheritdoc/>
        public Task Publish(object message)
        {
            #region Dispose and ArgumentException
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Cannot unsubscribe when disposed");
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var @event = (message is BusEvent) ? message as BusEvent : throw new ArgumentException(nameof(message));
            #endregion

            return _forceClient.CreateRecord(@event.Name, @event.Name);

        }

        private T GetListerner<T>()
        {
            foreach (var item in _messageListerners)
            {
                if (item.GetType() == typeof(T))
                {
                    _logger.LogInformation("Found match");
                    return (T)item;
                }
            }

            throw new ApplicationException($"{typeof(T).Name} hander is not found in the registry");
        }

        private string GetEventOrTopicName(string eventName)
        {
            return $"{_config.EventOrTopicUri}/{eventName}";
        }

        #region Dispose

        private bool _isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _streamingClient?.Dispose();
                _isDisposed = true;
            }
        }

        ~SalesforceEventBus()
        {
            Dispose(false);
        }

        #endregion
    }
}
