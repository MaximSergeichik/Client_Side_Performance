using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Client_Side
{
    class TestAction
    {
        public static Boolean useWaiting;

        public string type { get; set; }
        public string value { get; set; }
        public string attribute { get; set; }
        public string name { get; set; }
        public string waitFor { get; set; }

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
                if (!useWaiting)
                {
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
                else
                {
                    //TimeSpan ts = new TimeSpan(0, 3, 0);
                    //WebDriverWait wait = new WebDriverWait(driver, ts);
                    switch (type)
                    {
                        case "navigate":
                            {
                                Boolean fl = true;
                                DateTime start = DateTime.Now;
                                driver.Navigate().GoToUrl(value);
                                while (fl)
                                {
                                    try
                                    {
                                        driver.FindElement(By.XPath(waitFor));
                                        fl = false;
                                    }
                                    catch (Exception ex) { }
                                }
//                                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(waitFor)));
                                DateTime end = DateTime.Now;
                                int time = (end-start).Milliseconds;
                                WriteData.SendData(TestActionHelp.GetWaitTime(driver, name, time));
                                break;
                            }
                        case "click":
                            {
                                Boolean fl = true;
                                DateTime start = DateTime.Now;
                                driver.FindElement(By.XPath(value)).Click();
                                while (fl)
                                {
                                    try
                                    {
                                        driver.FindElement(By.XPath(waitFor));
                                        fl = false;
                                    }
                                    catch (Exception ex) { }
                                }
//                                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(waitFor)));
                                DateTime end = DateTime.Now;
                                int time = (end - start).Milliseconds;
                                if (attribute == "true")
                                {
                                    WriteData.SendData(TestActionHelp.GetWaitTime(driver, name, time));
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
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.Message);
            }
        }
    }
}
