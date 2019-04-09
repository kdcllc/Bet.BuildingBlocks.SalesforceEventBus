using Bet.BuildingBlocks.Abstractions;
using CometD.NetCore.Bayeux;
using CometD.NetCore.Bayeux.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using TestApp.Models.Salesforce;

namespace TestApp.EventBus.Messages
{
    /// <summary>
    /// The <see cref="CustomMessageListener"/> implements <see cref="IMessageListener"/>
    /// </summary>
    public class CustomMessageListener : IMessageListener
    {
        private readonly ILogger<CustomMessageListener> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor for <see cref="CustomMessageListener"/>.
        /// </summary>
        /// <param name="logger">Instance of the <see cref="ILogger{CustomMessageListener}"/>.</param>
        /// <param name="serviceProvider"></param>
        public CustomMessageListener(
            ILogger<CustomMessageListener> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Receives Salesforce message from Platform Event
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            var msg = JsonConvert.DeserializeObject<CustomMessageEnvelope>(message.Json);

            _logger.LogDebug($"{nameof(CustomMessageListener)} payload: {message.Json}");

            var custName = msg.Data.Payload.CustomerName;
            var replayId = msg.Data.Event.ReplayId;

            _logger.LogDebug($"Customer Name: {custName} - ReplayId: {replayId}");

            var busEvent = _serviceProvider.GetRequiredService<IEventBus>();

            // publishes to SF custom Object.
            busEvent.Publish<TestEvent__c>(new BusEvent<TestEvent__c>
            {
                Name = nameof(TestEvent__c),
                Data = new TestEvent__c { Name = custName }
            }).GetAwaiter().GetResult();
        }
    }
}
