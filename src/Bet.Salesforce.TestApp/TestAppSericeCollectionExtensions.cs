using Bet.Salesforce.TestApp.EventBus.Messages;
using Bet.Salesforce.TestApp.Services;

using CometD.NetCore.Bayeux.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TestAppSericeCollectionExtensions
    {
        public static IServiceCollection AddCustomerSalesforceEventBus(this IServiceCollection services)
        {
            services.AddHostedService<SalesforceEventBusHostedService>();

            services.AddTransient<IMessageListener, CustomMessageListener>();

            return services;
        }
    }
}
