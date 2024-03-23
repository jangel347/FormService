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

namespace FormService.Worker
{
    public class WorkerRobot
    {
        WorkerSettings _config;
        DataInput _data;
        List<Element> _elements;
        public bool isExecuted;
        static Logger log;
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

                driver.Navigate().GoToUrl("https://forms.office.com/Pages/ResponsePage.aspx?id=qNNG1hV7U0udkpgV_WJwLCfVP6r0EmVGnBeu-bvLR8VUNDlCNUVZNTFETTlWUjRKQjlYRTNSQzExSi4u");
                Thread.Sleep(3000);
                foreach (DataElement data_element in _data.dataElements)
                {
                    Element element = _elements.FirstOrDefault(element => element.idElement == data_element.elementId);
                    if (element == null) break;
                    PerformAction(driver, element, data_element.text);
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
