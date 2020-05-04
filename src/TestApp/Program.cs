using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestApp
{
    internal sealed class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                 .ConfigureHostConfiguration(configHost =>
                 {
                     configHost.SetBasePath(Directory.GetCurrentDirectory());
                     configHost.AddJsonFile("hostsettings.json", optional: true);
                     configHost.AddEnvironmentVariables(prefix: "TESTAPP_");
                     configHost.AddCommandLine(args);
                 })
                 .ConfigureAppConfiguration((hostContext, configBuilder) =>
                 {
                     configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                     configBuilder.AddJsonFile(
                         $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                         optional: true);

                     configBuilder.AddAzureKeyVault(hostingEnviromentName: hostContext.HostingEnvironment.EnvironmentName);

                     configBuilder.AddEnvironmentVariables(prefix: "TESTAPP_");
                     configBuilder.AddCommandLine(args);

                     if (hostContext.HostingEnvironment.IsDevelopment())
                     {
                         // print out the environment
                         var config = configBuilder.Build();
                         config.DebugConfigurations();
                     }
                 })
                 .ConfigureServices((context, services) =>
                 {
                     services.AddSalesforceEventBus();

                     services.AddCustomerSalesforceEventBus();

                     // Conjure up a RequestServices
                     services.AddTransient<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();
                 })
                 .ConfigureLogging((hostContext, configLogging) =>
                 {
                     configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                     configLogging.AddConsole();
                     configLogging.AddDebug();
                 })
                 .UseConsoleLifetime()
                 .Build();

            var sp = host.Services;

            await host.RunAsync();
        }
    }
}
