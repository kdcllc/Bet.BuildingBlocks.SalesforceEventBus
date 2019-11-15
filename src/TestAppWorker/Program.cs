using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestAppWorker
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                configBuilder.AddAzureKeyVault(hostingEnviromentName: hostContext.HostingEnvironment.EnvironmentName);

                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    // print out the environment
                    var config = configBuilder.Build();
                    config.DebugConfigurations();
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSalesforceEventBus(hostContext.Configuration);
                services.AddCustomerSalesforceEventBus();
            })
            .ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                configLogging.AddConsole();
                configLogging.AddDebug();
            });
        }
    }
}
