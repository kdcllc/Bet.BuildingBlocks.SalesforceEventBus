using Bet.BuildingBlocks.Abstractions;
using Bet.BuildingBlocks.SalesforceEventBus;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// An extension method for <see cref="SalesforceEventBus"/>.
    /// </summary>
    public static class SalesforceEventBusExtensions
    {
        /// <summary>
        /// Registers <see cref="SalesforceEventBus"/> and its dependencies from <see cref="StreamingClientExtensions"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddSalesforceEventBus(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "Salesforce")
        {
            services.AddResilientStreamingClient(configuration, sectionName);
            services.AddSingleton<IEventBus, SalesforceEventBus>();

            return services;
        }
    }
}
