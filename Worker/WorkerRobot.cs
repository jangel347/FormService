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
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                Thread.Sleep(_config.time_to_wait*1000);
                Boolean flagScreenshot = true;
                DateTime now = DateTime.Now;
                string fileName = "screenshot_" + now.ToString("yyyy-MM-dd_HH-mm-ss");
                foreach (DataElement data_element in _data.dataElements)
                {
                    Element element = _elements.FirstOrDefault(element => element.idElement == data_element.elementId);
                    if (element == null) break;
                    PerformAction(driver, element, data_element.text, wait);
                    if (flagScreenshot) { 
                        Screenshot screenshot =  driver.GetScreenshot();
                        try
                        {
                            Files.createDirectoryIfNotExists(PATH_SCREENSHOTS);
                            screenshot.SaveAsFile(PATH_SCREENSHOTS + fileName+"_1.jpg");
                        }
                        catch (Exception ex) {
                            log.WriteLog("Error al guardar pantallazo1: " + ex.Message, "ERROR");
                            throw ex;
                        }
                        flagScreenshot = false;
                    }
                }

                var submitButton = driver.FindElement(By.XPath(_config.submit_button));
                submitButton.Click();
                log.WriteLog($"Click en botón ENVIAR", "INFO");
                Thread.Sleep(_config.time_to_wait * 1000);
                Screenshot screenshot2 = driver.GetScreenshot();
                try
                {
                    Files.createDirectoryIfNotExists(PATH_SCREENSHOTS);
                    screenshot2.SaveAsFile(PATH_SCREENSHOTS + fileName + "_2.jpg");
                }
                catch (Exception ex)
                {
                    log.WriteLog("Error al guardar pantallazo1: " + ex.Message, "ERROR");
                    throw ex;
                }
                driver.Quit();
                Console.WriteLine("TERMINA ROBOT");
                log.WriteLog("TERMINA ROBOT ............................", "INFO");
                isExecuted = true;
            } catch (Exception ex)
            {
                log.WriteLog("Error con robot: " + ex.Message,"ERROR");
            }
        }

        private static void PerformAction(IWebDriver driver, Element element, string text, WebDriverWait wait)
        {
            var elementToFind = wait.Until(c => c.FindElement(By.XPath(element.XpathElement.Replace("_VARIABLE_", text))));
            //var elementToFind = driver.FindElement(By.XPath(element.XpathElement));
            //IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='elementoId']"));

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
