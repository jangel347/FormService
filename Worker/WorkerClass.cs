using System;
using System.ServiceProcess;
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
        static Logger log;
        public WorkerClass()
        {
            log = new Logger();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WorkerSettings _settings = Program.configuration.GetSection("WorkerService").Get<WorkerSettings>();
            if (_settings == null) throw new ArgumentException("No se cargaron correctamente las configuraciones");
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            // Convertir las variables de string a TimeSpan
            TimeSpan startTime = TimeSpan.Parse(_settings.time_limit1); 
            TimeSpan endTime = TimeSpan.Parse(_settings.time_limit2);
            if (_settings == null)
            {
                log.WriteLog("No sé cargó correctamente el archivo de configuración", "ERROR");
                Console.WriteLine("No sé cargó correctamente el archivo de configuración");
                return;
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime.TimeOfDay >= startTime && currentTime.TimeOfDay <= endTime) { 
                
                    List<Element> elements = _settings.elements.ToList();
                    int countRobot = 0;
                    foreach (DataInput data in _settings.data)
                    {
                        WorkerRobot robot = new WorkerRobot(_settings, data, elements);
                        if (robot.isExecuted) countRobot++;
                    }
                    if (countRobot > 0)
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        cancellationTokenSource.Cancel();
                        break;
                    }
                    else {
                        Console.WriteLine("El robot no pudo ser ejecutado");
                    }
                }
                await Task.Delay(10000, stoppingToken);
            }
            //ServiceController serviceController = new ServiceController("FormService");
            //serviceController.Pause();
            Console.WriteLine("Presiona una la tecla ENTER para salir");
            Console.Read();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}