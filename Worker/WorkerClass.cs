using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FormService;
using FormService.Templates;
using FormService.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
namespace Worker
{
    public class WorkerClass : BackgroundService
    {

        public WorkerClass()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WorkerSettings _settings = Program.configuration.GetSection("WorkerService").Get<WorkerSettings>();
            if (_settings == null)
            {
                Console.WriteLine("No s� carg� correctamente el archivo de configuraci�n");
                return;
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                List<Element> elements = _settings.elements.ToList();
                foreach (DataInput data in _settings.data)
                {
                    WorkerRobot robot = new WorkerRobot(_settings, data, elements);
                }
                await Task.Delay(8000, stoppingToken);
            }
        }
    }
}