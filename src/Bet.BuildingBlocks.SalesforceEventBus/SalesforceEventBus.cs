using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bet.BuildingBlocks.Abstractions;

using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Salesforce;
using CometD.NetCore.Salesforce.ForceClient;
using CometD.NetCore.Salesforce.Resilience;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bet.BuildingBlocks.SalesforceEventBus
{
    /// <summary>
    /// The <see cref="SalesforceEventBus"/> provides a way to register events for Salesforce CometD communication bus.
    /// </summary>
    public class SalesforceEventBus : IEventBus
    {
        private readonly IStreamingClient _streamingClient;
        private readonly ILogger<SalesforceEventBus> _logger;
        private readonly IResilientForceClient _forceClient;
        private readonly SalesforceConfiguration _options;
        private readonly IEnumerable<IMessageListener> _messageListerners;

        private readonly ConcurrentDictionary<SubscriptionInfo, object> _subscriptions =
            new ConcurrentDictionary<SubscriptionInfo, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesforceEventBus"/> class.
        /// </summary>
        /// <param name="streamingClient">The instance of <see cref="IStreamingClient"/> with connection to salesforce.</param>
        /// <param name="logger">The instance of <see cref="ILogger{SalesforceEventBus}"/>.</param>
        /// <param name="forceClient">The instance of <see cref="ForceClientProxy"/> to provide a publish functionality to the bus.</param>
        /// <param name="messageListeners"></param>
        /// <param name="options"></param>
        public SalesforceEventBus(
            IStreamingClient streamingClient,
            ILogger<SalesforceEventBus> logger,
            IResilientForceClient forceClient,
            IEnumerable<IMessageListener> messageListeners,
            IOptions<SalesforceConfiguration> options)
        {
            _streamingClient = streamingClient ?? throw new ArgumentNullException(nameof(streamingClient));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _forceClient = forceClient ?? throw new ArgumentNullException(nameof(forceClient));

            _options = options.Value ?? throw new ArgumentNullException(nameof(options));

            _messageListerners = messageListeners;

            _streamingClient.Reconnect += StreamingClient_Reconnect;

            _streamingClient.Handshake();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SalesforceEventBus"/> class.
        /// </summary>
        ~SalesforceEventBus()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public Task Subscribe<T>(BusEvent<T> eventMessage) where T : class
        {
            if (eventMessage == null)
            {
                throw new ArgumentNullException(nameof(eventMessage));
            }

            var @event = (eventMessage is PlatformEvent<T>) ?
                eventMessage as PlatformEvent<T> :
                throw new ArgumentException($"{nameof(eventMessage)} must be type of {nameof(PlatformEvent<T>)}");

            if (!typeof(IMessageListener).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException("Type parameter must be an IMessageListener");
            }

            var eventName = eventMessage.Name;
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException($"EventName of {nameof(eventMessage)} cannot be empty");
            }

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

            _streamingClient.SubscribeTopic(topicName, handler as IMessageListener, @event.ReplayId);

            _logger.LogDebug($"{topicName} is subscribed with ReplayId: {@event.ReplayId}");

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task Unsubscribe<T>(BusEvent<T> eventMessage) where T : class
        {
            if (eventMessage == null)
            {
                throw new ArgumentNullException(nameof(eventMessage));
            }

            var @event = (eventMessage is PlatformEvent<T>) ?
              eventMessage as PlatformEvent<T> :
              throw new ArgumentException($"{nameof(eventMessage)} must be type of {nameof(PlatformEvent<T>)}");

            if (!typeof(IMessageListener).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException("Type parameter must be an IMessageListener");
            }

            var eventName = eventMessage.Name;
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException($"EventName of {nameof(eventMessage)} cannot be empty");
            }

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

        /// <inheritdoc/>
        public Task Publish<T>(BusEvent<T> message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(message.Name))
            {
                throw new ArgumentException(nameof(message));
            }

            // publish event back to Salesforce
            return _forceClient.CreateRecordAsync<T>(message.Name, message.Data);
        }

        /// <inheritdoc/>
        public Task Publish<T>(object message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var @event = (message is BusEvent<T>) ? message as BusEvent<T> : throw new ArgumentException(nameof(message));

            return _forceClient.CreateRecordAsync<T>(@event.Name, @event.Data);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _streamingClient?.Dispose();
            }
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
            return $"{_options.EventOrTopicUri}/{eventName}";
        }

        private void StreamingClient_Reconnect(object sender, bool isReconnected)
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
    }
}
