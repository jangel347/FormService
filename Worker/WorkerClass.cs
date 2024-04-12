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
using static System.Runtime.InteropServices.JavaScript.JSType;
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
            try
            {
                WorkerSettings _settings = Program.configuration.GetSection("WorkerService").Get<WorkerSettings>();
                if (_settings == null) throw new ArgumentException("No se cargaron correctamente las configuraciones");
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                // Convertir las variables de string a TimeSpan
                TimeSpan startTime = TimeSpan.Parse(_settings.time_limit1);
                TimeSpan endTime = TimeSpan.Parse(_settings.time_limit2);
                log.WriteLog($"Ejecución entre {_settings.time_limit1} y {_settings.time_limit2}", "INFO");
                Console.WriteLine($"Ejecución entre {_settings.time_limit1} y {_settings.time_limit2}");
                if (_settings == null)
                {
                    log.WriteLog("No sé cargó correctamente el archivo de configuración", "ERROR");
                    Console.WriteLine("No sé cargó correctamente el archivo de configuración");
                    return;
                }
                bool logged = false;
                WorkerRobot robot = new WorkerRobot(_settings);
                logged = robot.Login();
                while (!stoppingToken.IsCancellationRequested && logged)
                {
                    DateTime currentTime = DateTime.Now;
                    int count = 1;
                    if (currentTime.TimeOfDay >= startTime && currentTime.TimeOfDay <= endTime)
                    {

                        List<Element> elements = _settings.elements.ToList();
                        int countRobot = 0;
                        foreach (DataInput data in _settings.data)
                        {
                            robot = new WorkerRobot(_settings, data, elements, robot._driver, robot._isSessionActive);
                            if (count == _settings.data.Length) robot.isLast = true;
                            robot.ExecuteRobot();
                            if (robot.isExecuted) countRobot++;
                            count++;
                        }
                        if (countRobot > 0)
                        {
                            cancellationTokenSource.Token.ThrowIfCancellationRequested();
                            cancellationTokenSource.Cancel();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("El robot no pudo ser ejecutado");
                        }
                    }
                    await Task.Delay(10000, stoppingToken);

                }
                //ServiceController serviceController = new ServiceController("FormService");
                //serviceController.Pause();
                if (!logged) {
                    log.WriteLog("Fallo al iniciar sesión XXXXXXXXXXXXXXXXXXXXXXXXXXX", "ERROR");
                }
                Console.WriteLine("Presiona una la tecla ENTER para salir");
                Console.Read();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                log.WriteLog("Algo está mal: " + ex.Message, "ERROR");
            }
            
        }
    }
}