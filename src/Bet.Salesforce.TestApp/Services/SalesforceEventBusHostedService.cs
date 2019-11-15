using System.Threading;
using System.Threading.Tasks;

using Bet.BuildingBlocks.Abstractions;
using Bet.Salesforce.TestApp.EventBus.Messages;

using CometD.NetCore.Salesforce;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bet.Salesforce.TestApp.Services
{
    /// <summary>
    /// Provides with LifetimeEventsHostedService.
    /// </summary>
    public class SalesforceEventBusHostedService : IHostedService
    {
        private readonly ILogger _logger;
#if NETSTANDARD2_0
        private readonly IApplicationLifetime _appLifetime;
#elif NETSTANDARD2_1
        private readonly IHostApplicationLifetime _appLifetime;
#endif
        private readonly SalesforceConfiguration _options;
        private readonly IEventBus _eventBus;

        public SalesforceEventBusHostedService(
            ILogger<SalesforceEventBusHostedService> logger,
#if NETSTANDARD2_0
            IApplicationLifetime appLifetime,
#elif NETSTANDARD2_1
            IHostApplicationLifetime appLifetime,
#endif
            IOptions<SalesforceConfiguration> options,
            IEventBus eventBus)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _options = options.Value;
            _eventBus = eventBus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            _logger.LogInformation("StartAsync has been called.");

            await _eventBus.Subscribe<CustomMessageListener>(
               new PlatformEvent<CustomMessageListener>()
               {
                   Name = _options.CustomEvent,
                   ReplayId = _options.ReplayId
               });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync has been called.");

            await _eventBus.Unsubscribe<CustomMessageListener>(
                new PlatformEvent<CustomMessageListener>()
                {
                    Name = _options.CustomEvent,
                    ReplayId = _options.ReplayId
                });
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }
}
