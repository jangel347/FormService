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
        public bool _isSessionActive;
        static Logger log;
        static string PATH_SCREENSHOTS = "C:\\FormService\\Screenshots\\";
        public ChromeDriver _driver;
        public bool isLast { get; set; }
        string fileName;

        public WorkerRobot(WorkerSettings config) {
            _config = config;
            _driver = null;
            _isSessionActive = false;
        }
        public WorkerRobot(WorkerSettings config, DataInput data, List<Element> elements, ChromeDriver driver, bool isSessionActive) { 
            _config = config;
            _data = data;
            _elements = elements;
            isExecuted = false;
            isLast = false;
            _isSessionActive = isSessionActive;
            _driver = driver;
            log = new Logger();
        }

        public void ExecuteRobot(){
            try {
                DateTime now = DateTime.Now;
                fileName = "screenshot_" + now.ToString("yyyy-MM-dd_HH-mm-ss");
                log.WriteLog("EJECUTA ROBOT ............................", "INFO");
                Console.WriteLine("EJECUTA ROBOT");
                if (_driver == null)
                    _driver = new ChromeDriver();
                Files.createDirectoryIfNotExists(PATH_SCREENSHOTS);
                _driver.Navigate().GoToUrl(_config.url);
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                Thread.Sleep(_config.time_to_wait*1000);
                Boolean flagScreenshot = true;
                
                foreach (DataElement data_element in _data.dataElements)
                {
                    Element element = _elements.FirstOrDefault(element => element.idElement == data_element.elementId);
                    if (element == null) break;
                    PerformAction(element, data_element.text, wait);
                    if (flagScreenshot) { 
                        Screenshot screenshot =  _driver.GetScreenshot();
                        try
                        {
                            screenshot.SaveAsFile(PATH_SCREENSHOTS + fileName+"_1.jpg");
                        }
                        catch (Exception ex) {
                            log.WriteLog("Error al guardar pantallazo1: " + ex.Message, "ERROR");
                            throw ex;
                        }
                        flagScreenshot = false;
                    }
                }

                var submitButton = _driver.FindElement(By.XPath(_config.submit_button));
                submitButton.Click();
                log.WriteLog($"Click en botón ENVIAR", "INFO");
                Thread.Sleep(_config.time_to_wait * 1000);
                Screenshot screenshot2 = _driver.GetScreenshot();
                try
                {
                    screenshot2.SaveAsFile(PATH_SCREENSHOTS + fileName + "_2.jpg");
                }
                catch (Exception ex)
                {
                    log.WriteLog("Error al guardar pantallazo2: " + ex.Message, "ERROR");
                    throw ex;
                }
                Thread.Sleep(_config.time_to_wait * 1000);
                var saveButton = _driver.FindElement(By.XPath(_config.save_button));
                saveButton.Click();
                if (isLast) _driver.Quit();
                Console.WriteLine("TERMINA ROBOT");
                log.WriteLog("TERMINA ROBOT ............................", "INFO");
                isExecuted = true;
            } catch (Exception ex)
            {
                log.WriteLog("Error con robot: " + ex.Message,"ERROR");
            }
        }

        private static void PerformAction(Element element, string text, WebDriverWait wait)
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
            Thread.Sleep(500);
        }

        public bool Login() {
            try
            {
                if (_driver == null)
                    _driver = new ChromeDriver();
                _driver.Navigate().GoToUrl(_config.account.url_form);
                Thread.Sleep(120000);
                /*WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                DateTime now = DateTime.Now;
                fileName = "screenshot_" + now.ToString("yyyy-MM-dd_HH-mm-ss");
                //user
                Thread.Sleep(_config.time_to_wait * 1000);
                var submitButton = _driver.FindElement(By.XPath(_config.account.email_element));
                var email_element = wait.Until(c => c.FindElement(By.XPath(_config.account.email_element)));
                var email_btn_element = wait.Until(c => c.FindElement(By.XPath(_config.account.email_btn_element)));
                email_element.SendKeys(_config.account.email);
                Thread.Sleep(500);
                email_btn_element.Click();
                //password
                Thread.Sleep(_config.time_to_wait * 1000);
                var pass_element = wait.Until(c => c.FindElement(By.XPath(_config.account.password_element)));
                var pass_btn_element = wait.Until(c => c.FindElement(By.XPath(_config.account.password_btn_element)));
                pass_element.SendKeys(_config.account.password);
                Thread.Sleep(500);
                pass_btn_element.Click();
                Thread.Sleep(_config.time_to_wait * 1000);
                var session_chk_element = wait.Until(c => c.FindElement(By.XPath(_config.account.password_element)));
                var session_btn_element = wait.Until(c => c.FindElement(By.XPath(_config.account.password_btn_element)));
                session_chk_element.Click();
                Thread.Sleep(500);
                session_btn_element.Click();
                Thread.Sleep(_config.time_to_wait * 1000);
                log.WriteLog("SESIÓN INICIADA ............................", "INFO");
                Screenshot screenshot = _driver.GetScreenshot();
                try
                {
                    screenshot.SaveAsFile(PATH_SCREENSHOTS + fileName + "_LOGIN.jpg");
                }
                catch (Exception ex)
                {
                    log.WriteLog("Error al guardar pantallazo1: " + ex.Message, "ERROR");
                    throw ex;
                }
                Thread.Sleep(_config.time_to_wait * 1000);*/
            }
            catch (Exception ex) {
                log.WriteLog("Error al iniciar sesión: " + ex.Message, "ERROR");
                return false;
            }
            Thread.Sleep(_config.time_to_wait * 1000);
            return true;

        }
    }
}
