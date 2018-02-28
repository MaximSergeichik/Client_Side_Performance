using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Client_Side
{
    class TestAction
    {
        public static Boolean useWaiting;
        public static Boolean isDebug;

        public string type { get; set; }
        public string value { get; set; }
        public string attribute { get; set; }
        public string name { get; set; }
        public string waitFor { get; set; }
        public string className { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string measure { get; set; }
        public string tag { get; set; }

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
                                if (measure == "true")
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
                        case "thinkTime":
                            {
                                if (isDebug)
                                {
                                    Thread.Sleep(Convert.ToInt32(value));
                                }
                                break;
                            }
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
                                int time = Convert.ToInt32((end-start).TotalMilliseconds);
                                WriteData.SendData(TestActionHelp.GetWaitTime(driver, name, time));
                                break;
                            }
                        case "click":
                            {
                                IWebElement n;
                                if (className != null)
                                {
                                    List<IWebElement> list = driver.FindElements(By.ClassName(className)).ToList();
                                    n = list.Where(i => i.Text == text).First();
                                }
                                else
                                {
                                    if (tag != null)
                                    {
                                        List<IWebElement> list = driver.FindElements(By.TagName(tag)).ToList();
                                        n = list.Where(i => i.GetAttribute("title") == title).First();
                                    }
                                    else
                                    {
                                        n = driver.FindElement(By.XPath(value));
                                    }
                                }
                                if (measure == "true")
                                {
                                    Boolean fl = true;
                                    DateTime start = DateTime.Now;
                                    n.Click();
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
                                    int time = Convert.ToInt32((end - start).TotalMilliseconds);
                                
                                    WriteData.SendData(TestActionHelp.GetWaitTime(driver, name, time));
                                }
                                else
                                {
                                    n.Click(); 
                                }
                                break;
                            }
                        case "sendKeys":
                            {
                                Random r = new Random();
                                IWebElement n;
                                if (text.Contains("randomNumb"))
                                {
                                    text = text.Replace("randomNumb", r.Next(1000000, 9999999).ToString());
                                }
                                if (text.Contains("randomStr"))
                                {
                                    text = text.Replace("randomStr", Path.GetRandomFileName().Replace(".",""));
                                }
                                if (className != null)
                                {
                                    List<IWebElement> list = driver.FindElements(By.ClassName(className)).ToList();
                                    n = list.Where(i => i.GetAttribute("title").Equals(title)).First().FindElement(By.TagName("input"));
                                }
                                else
                                {
                                    n = driver.FindElement(By.XPath(value));
                                }
                                Thread.Sleep(1000);

                                string s = n.GetAttribute("value");
                                string s1 = "";
                                for (int i = 0; i < s.Length;i++ )
                                {
                                    s1 += "\b";
                                }
                                n.SendKeys(s1);
                                n.SendKeys(text);
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
