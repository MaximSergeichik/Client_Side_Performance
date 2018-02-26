using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Client_Side
{
    class TestAction
    {
        public static Boolean useWaiting;

        public string type { get; set; }
        public string value { get; set; }
        public string attribute { get; set; }
        public string name { get; set; }

        public TestAction() { }

        public void Show()
        {
            switch (type)
            {
                case "navigate": { Program.ShowMessage(String.Format("Command: {0} URL: {1} Name: {2}", type, value, name)); break; }
                case "click": { Program.ShowMessage(String.Format("Command: {0} xPath: {1} IsNewPage: {2} Name: {3}", type, value, attribute, name)); break; }
                case "sendKeys": { Program.ShowMessage(String.Format("Command: {0} xPath: {1} string: {2}", type, value, attribute)); break; }
            }
        }

        public void Perform()
        {
            try
            {
                IWebDriver driver = WebDriver.Driver;
                switch (type)
                {
                    case "navigate":
                        {
                            driver.Navigate().GoToUrl(value);
                            WriteData.SendData(TestActionHelp.GetTimes(driver, name));
                            break;
                        }
                    case "click":
                        {
                            driver.FindElement(By.XPath(value)).Click();
                            if (attribute == "true")
                            {
                                WriteData.SendData(TestActionHelp.GetTimes(driver, name));
                            }
                            break;
                        }
                    case "sendKeys":
                        {
                            driver.FindElement(By.XPath(value)).SendKeys(attribute);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.Message);
            }
        }
    }
}
