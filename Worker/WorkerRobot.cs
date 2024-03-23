using FormService.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FormService.Worker
{
    public class WorkerRobot
    {
        WorkerSettings _config;
        DataInput _data;
        List<Element> _elements;
        public WorkerRobot(WorkerSettings config, DataInput data, List<Element> elements) { 
            _config = config;
            _data = data;
            _elements = elements;
            Robot();
        }

        private void Robot(){
            Console.WriteLine("EJECUTA ROBOT");
            var driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://forms.office.com/Pages/ResponsePage.aspx?id=qNNG1hV7U0udkpgV_WJwLCfVP6r0EmVGnBeu-bvLR8VUNDlCNUVZNTFETTlWUjRKQjlYRTNSQzExSi4u");

            foreach (DataElement data_element in _data.dataElements)
            {
                Element element = _elements.FirstOrDefault(element => element.idElement == data_element.elementId);
                if (element == null) break;
                PerformAction(driver, element, data_element.text);
            }

            var submitButton = driver.FindElement(By.XPath(_config.submit_button));
            submitButton.Click();

            driver.Quit();
            Console.WriteLine("TERMINA ROBOT");
        }

        private static void PerformAction(IWebDriver driver, Element element, string text)
        {
            var elementToFind = driver.FindElement(By.XPath(element.XpathElement));

            switch (element.Action)
            {
                case "click":
                    elementToFind.Click();
                    Console.WriteLine("=> Realizando Click");
                    break;
                case "type":
                    elementToFind.SendKeys(text);
                    Console.WriteLine($"=> Escribiendo: {text}");
                    break;
                default:
                    Console.WriteLine($"=> Acción no reconocida: {element.Action}");
                    break;
            }
        }
    }
}
