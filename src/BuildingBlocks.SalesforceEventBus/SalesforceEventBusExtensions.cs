using KDCLLC.BuildingBlocks.Abstractions;
using KDCLLC.BuildingBlocks.SalesforceEventBus;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// An extension method for <see cref="SalesforceEventBus"/>.
    /// </summary>
    public static class SalesforceEventBusExtensions
    {
        /// <summary>
        /// Registers <see cref="SalesforceEventBus"/> and its dependencies from <see cref="StreamingClientExtensions"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSalesforceEventBus(this IServiceCollection services)
        {
            services.AddStreamingClient();
            services.AddSingleton<IEventBus, SalesforceEventBus>();

            return services;
        }
    }
}
