using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Worker;

namespace FormService
{
    public class Program
    {
        public static IConfiguration configuration;
        public static async Task Main(string[] args)
        {
            var buildConfig = new ConfigurationBuilder()
                .AddJsonFile("C:\\FormService\\appsettings.json", optional : true, reloadOnChange : true);

            configuration = buildConfig.Build();
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configApp.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WorkerClass>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}