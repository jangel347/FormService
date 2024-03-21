using FormService.Templates;
using FormService.Worker;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ConfigTemplate>(hostContext.Configuration.GetSection("Config"),
                                                   options => options.BindNonPublicProperties = true);
                services.AddSingleton<ConfigTemplate>();
                services.AddHostedService<Worker>();
            })
            .UseWindowsService();

        var host = builder.Build();
        host.Run();
    }
}