using FormService.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using FormService.Utilities;

namespace FormService.Worker
{
    public class WorkerRobot
    {
        WorkerSettings _config;
        DataInput _data;
        List<Element> _elements;
        public bool isExecuted;
        static Logger log;
        static string PATH_SCREENSHOTS = "C:\\FormService\\Screenshots\\";
        public WorkerRobot(WorkerSettings config, DataInput data, List<Element> elements) { 
            _config = config;
            _data = data;
            _elements = elements;
            isExecuted = false;
            log = new Logger();
            Robot();
        }

        private void Robot(){
            try {
                log.WriteLog("EJECUTA ROBOT ............................", "INFO");
                Console.WriteLine("EJECUTA ROBOT");
                var driver = new ChromeDriver();

                driver.Navigate().GoToUrl(_config.url);
                Thread.Sleep(_config.time_to_wait*1000);
                Boolean flagScreenshot = true;
                foreach (DataElement data_element in _data.dataElements)
                {
                    Element element = _elements.FirstOrDefault(element => element.idElement == data_element.elementId);
                    if (element == null) break;
                    PerformAction(driver, element, data_element.text);
                    if (flagScreenshot) { 
                        Screenshot screenshot =  driver.GetScreenshot();
                        try
                        {
                            DateTime now = DateTime.Now;
                            string fileName = "screenshot_"+now.ToString("yyyy-MM-dd_HH-mm-ss")+".jpg";
                            Files.createDirectoryIfNotExists(PATH_SCREENSHOTS);
                            screenshot.SaveAsFile(PATH_SCREENSHOTS + fileName);
                        }
                        catch (Exception ex) {
                            log.WriteLog("Error al guardar pantallazo: " + ex.Message, "ERROR");
                            throw ex;
                        }
                        flagScreenshot = false;
                    }
                }

                var submitButton = driver.FindElement(By.XPath(_config.submit_button));
                submitButton.Click();
                log.WriteLog($"Click en botón ENVIAR", "INFO");
                driver.Quit();
                Console.WriteLine("TERMINA ROBOT");
                log.WriteLog("TERMINA ROBOT ............................", "INFO");
                isExecuted = true;
            } catch (Exception ex)
            {
                log.WriteLog("Error con robot: " + ex.Message,"ERROR");
            }
        }

        private static void PerformAction(IWebDriver driver, Element element, string text)
        {
            var elementToFind = driver.FindElement(By.XPath(element.XpathElement));
            switch (element.Action)
            {
                case "click":
                    elementToFind.Click();
                    Console.WriteLine($"Realizando Click en {element.NameElement}");
                    
                    log.WriteLog($"Realizando Click en {element.NameElement}", "INFO");
                    break;
                case "type":
                    elementToFind.SendKeys(text);
                    Console.WriteLine($"Escribiendo '{text}' en {element.NameElement}");
                    log.WriteLog($"Escribiendo '{text}' en {element.NameElement}", "INFO");
                    break;
                default:
                    Console.WriteLine($"Acción no reconocida: {element.Action}");
                    break;
            }
        }
    }
}
