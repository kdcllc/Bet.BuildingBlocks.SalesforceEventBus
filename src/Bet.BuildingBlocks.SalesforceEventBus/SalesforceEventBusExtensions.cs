using Bet.BuildingBlocks.Abstractions;
using Bet.BuildingBlocks.SalesforceEventBus;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// An extension method for <see cref="EventBus"/>.
    /// </summary>
    public static class SalesforceEventBusExtensions
    {
        /// <summary>
        /// Registers <see cref="EventBus"/> and its dependencies from <see cref="StreamingClientExtensions"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddSalesforceEventBus(
            this IServiceCollection services,
            string sectionName = "Salesforce")
        {
            services.AddResilientStreamingClient(sectionName);
            services.AddSingleton<IEventBus, EventBus>();

            return services;
        }
    }
}
