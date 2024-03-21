using System.IO;
using System.Threading.Tasks;

namespace FormService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ConfigTemplate _config;

        public Worker(ILogger<Worker> logger, ConfigTemplate config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    foreach (DataTemplate data in _config.data)
                    {
                        _logger.LogInformation("Worker running at: {time} " + data.numEjecutantes, DateTimeOffset.Now);
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}