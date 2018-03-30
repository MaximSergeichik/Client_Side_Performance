using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static string creditianals;
        public static int numb;

        public List<TestAction> innerActions = new List<TestAction>();


        public string type { get; set; }
        public string innerText { get; set; }
        public string attribute { get; set; }
        public string name { get; set; }
        public string measure { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public string iterations { get; set; }
        
        
        public string waitForXPath { get; set; }
        public string waitForClassName {get; set;}
        public string waitForTitle { get; set; }
        public string waitForTag { get; set; }
        public string waitForInTag { get; set; }
        public string waitForInLabel { get; set; }
        public string waitForInText { get; set; }
        public string waitForInValue { get; set; }
        public string waitForId { get; set; }

        public string objectXPath { get; set; }
        public string objectClassName { get; set; }
        public string objectTitle { get; set; }
        public string objectTag { get; set; }
        public string objectInTag { get; set; }
        public string objectInLabel { get; set; }
        public string objectInText { get; set; }
        public string objectInValue { get; set; }
        public string objectId { get; set; }

        public string sendInLog { get; set; }

        public TestAction() { }

        public IWebElement LocateElement(IWebDriver driver, string xPath, string className, string title, string tag, string inTag, string inLabel, string inText, string inValue, string id)
        {
            IWebElement n = null;

            if (tag != null)
            {
                if (id != null)
                {
                    if (inText == null && inTag == null)
                    {
                        n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("id").Contains(id)).First();
                    }
                    if (inTag != null && inText != null)
                    {
                        if (inText.Contains("match"))
                        {
                            string pattern = inText.Split('|').ToArray()[1];
                            Regex r = new Regex(pattern);
                            n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("id").Contains(id)).Where(k => k.FindElements(By.XPath(inTag)).Where(j => r.IsMatch(j.Text)) != null).First();
                            inText = null;
                        }
                        else
                        {
                            n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("id").Contains(id)).First().FindElements(By.XPath(inTag)).Where(j => j.Text.Contains(inText)).First();
                            inText = null;
                        }
                    }
                }
                if (inValue != null)
                {
                    n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("value").Contains(inValue)).First();
                }
                if (title != null && inTag == null)
                {
                    n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("title").Contains(title)).First();
                }
                if (title != null && inTag != null)
                {
                    n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("title").Contains(title)).First().FindElement(By.XPath(inTag));
                }
                if (inTag != null && inLabel != null)
                {
                    List<IWebElement> list = driver.FindElements(By.TagName(tag)).ToList();
                    foreach(var a in list)
                    {
                        try
                        {
                            if (a.FindElement(By.TagName("label")).Text.Contains(inLabel))
                            {
                                n = a;
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                    n = n.FindElement(By.XPath(inTag));
                }
                if (inText != null) 
                {
                    if (!inText.Contains("match"))
                    {
                        n = driver.FindElements(By.TagName(tag)).Where(i => i.Text.Contains(inText)).First();
                    }
                    else
                    {
                        string pattern = inText.Split('|').ToArray()[1];
                        Regex r = new Regex(pattern);

                        File.WriteAllLines(@"D:/client_side", driver.FindElements(By.TagName(tag)).Select(i => i.Text));

                        n = driver.FindElements(By.TagName(tag)).Where(i => r.IsMatch(i.Text)).First();
                    }
                }
            }
            if (className != null)
            {
                if (inText != null)
                {
                    n = driver.FindElements(By.ClassName(className)).Where(i => i.Text.Contains(inText)).First();
                }
                if(inTag != null && title == null)
                {
                    n = driver.FindElements(By.ClassName(className)).Where(i => i.FindElements(By.XPath(inTag)).Count > 0).First().FindElement(By.XPath(inTag));
                }
                if (title != null && inTag == null)
                {
                    n = driver.FindElements(By.ClassName(className)).Where(i => i.GetAttribute("title").Contains(title)).First();
                }
                if (title != null && inTag != null)
                {
                    n = driver.FindElements(By.ClassName(className)).Where(i => i.GetAttribute("title").Contains(title)).First().FindElement(By.XPath(inTag));
                }
            }
            if (xPath!=null)
            {
                n = driver.FindElement(By.XPath(xPath));
            }
            return n;
        }

        public void Show()
        {
            switch (type)
            {
                case "navigate": { Program.ShowMessage(String.Format("Command: {0} URL: {1} Name: {2}", type, objectXPath, name)); break; }
                case "click": { Program.ShowMessage(String.Format("Command: {0} xPath: {1} IsNewPage: {2} Name: {3}", type, objectXPath, attribute, name)); break; }
                case "sendKeys": { Program.ShowMessage(String.Format("Command: {0} xPath: {1} string: {2}", type, objectXPath, attribute)); break; }
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
                                driver.Navigate().GoToUrl(objectXPath);
                                WriteData.SendData(TestActionHelp.GetTimes(driver, name));
                                break;
                            }
                        case "click":
                            {
                                driver.FindElement(By.XPath(objectXPath)).Click();
                                if (measure == "true")
                                {
                                    WriteData.SendData(TestActionHelp.GetTimes(driver, name));
                                }
                                break;
                            }
                        case "sendKeys":
                            {
                                driver.FindElement(By.XPath(objectXPath)).SendKeys(attribute);
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
                        case "refresh":
                            {
                                driver.Navigate().Refresh();
                                bool fl = true;
                                while (fl)
                                {
                                    try
                                    {
                                        LocateElement(driver, waitForXPath, waitForClassName, waitForTitle, waitForTag, waitForInTag, waitForInLabel, waitForInText, waitForInValue, waitForId);
                                        fl = false;
                                        //Console.WriteLine(DateTime.Now);
                                    }
                                    catch (Exception ex) { }
                                }
                                break;
                            }
                        case "wait":
                            {
                                bool fl = true;
                                while (fl)
                                {
                                    try
                                    {
                                        LocateElement(driver, waitForXPath, waitForClassName, waitForTitle, waitForTag, waitForInTag, waitForInLabel, waitForInText, waitForInValue, waitForId);
                                        fl = false;
                                        //Console.WriteLine(DateTime.Now);
                                    }
                                    catch (Exception ex) { }
                                }
                                break;
                            }
                        case "for":
                            {
                                int count = Int32.Parse(iterations);
                                for (int i=0;i<count;i++)
                                {
                                    innerActions.ForEach(k => k.Perform());
                                }
                                break;
                            }
                        case "if":
                            {
                                IWebElement n = LocateElement(driver, objectXPath, objectClassName, objectTitle, objectTag, objectInTag, objectInLabel, objectInText, objectInValue, objectId);
                                if (value != null)
                                {
                                    if (n.GetAttribute("value").Equals(value))
                                    {
                                        innerActions.ForEach(i => i.Perform());
                                    }
                                }
                                if (text != null)
                                {
                                    if (n.Text.Contains(text))
                                    {
                                        innerActions.ForEach(i => i.Perform());
                                    }
                                }
                                break;
                            }
                        case "thinkTime":
                            {
                                if (isDebug)
                                {
                                    Thread.Sleep(Convert.ToInt32(innerText));
                                }
                                break;
                            }
                        case "navigate":
                            {
                                Boolean fl = true;
                                DateTime start = DateTime.Now;
                                driver.Navigate().GoToUrl(innerText);
                                
                                while (fl)
                                {
                                    try
                                    {
                                        LocateElement(driver, waitForXPath, waitForClassName, waitForTitle, waitForTag, waitForInTag, waitForInLabel, waitForInText, waitForInValue, waitForId);
                                        fl = false;
                                        Console.WriteLine(DateTime.Now);
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
                                //File.WriteAllText(@"D:\Perfomance\ClientSide\Client_Side_Performance\check.txt", driver.PageSource);
                                IWebElement n = LocateElement(driver, objectXPath, objectClassName, objectTitle, objectTag, objectInTag, objectInLabel, objectInText, objectInValue, objectId);
                                if (measure == "true")
                                {
                                    Boolean fl = true;
                                    int time1 = 0;                                    


                                    if (!name.Contains("Logon") && !name.Contains("LogOff") && !name.Contains("StoreStockLookup"))
                                    {
                                        n.Click();
                                        while (fl)
                                        {
                                            try
                                            {
                                                LocateElement(driver, waitForXPath, waitForClassName, waitForTitle, waitForTag, waitForInTag, waitForInLabel, waitForInText, waitForInValue, waitForId);
                                                fl = false;
                                            }
                                            catch (Exception ex) { }
                                        }
                                        Thread.Sleep(5000);
                                        Boolean f = true;
                                        while (f)
                                        {
                                            if (driver.PageSource.Contains("Performance Statistics"))
                                            {
                                                f = false;
                                                Regex r = new Regex("end-to-end-time\":\"(.*?)s\"");
                                                Match m = r.Match(driver.PageSource);
                                                time1 = Convert.ToInt32(Convert.ToDouble(m.Groups[1].Value) * 1000);
                                            }
                                        }
                                        
                                        //DateTime end = DateTime.Now;
                                        //int time = Convert.ToInt32((end - start).TotalMilliseconds) - time1;

                                        WriteData.SendData(TestActionHelp.GetWaitTime(driver, name, time1));
                                    }
                                    else
                                    {
                                        DateTime start = DateTime.Now;
                                        n.Click();
                                        Boolean f = true;
                                        if (name.Contains("Logon"))
                                        {
                                            while (f)
                                            {
                                                if (driver.PageSource.Contains("Performance Statistics") && driver.PageSource.Contains("ui-step-type\":\"Logon"))
                                                {
                                                    f = false;
                                                    Regex r = new Regex("end-to-end-time\":\"(.*?)s\"");
                                                    Match m = r.Match(driver.PageSource);
                                                    time1 = Convert.ToInt32(Convert.ToDouble(m.Groups[1].Value) * 1000);
                                                }
                                            }
                                        }
                                        while (fl)
                                        {
                                            try
                                            {
                                                LocateElement(driver, waitForXPath, waitForClassName, waitForTitle, waitForTag, waitForInTag, waitForInLabel, waitForInText, waitForInValue, waitForId);
                                                fl = false;
                                            }
                                            catch (Exception ex) { }
                                        }
                                        //                                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(waitFor)));
                                        DateTime end = DateTime.Now;
                                        int time = Convert.ToInt32((end - start).TotalMilliseconds) - time1;

                                        if (name.Contains("Logon"))
                                        {
                                            //WriteData.SendData(TestActionHelp.GetWaitTime(driver, "CallPad", time));
                                            WriteData.SendData(TestActionHelp.GetWaitTime(driver, "Logon", time1));
                                        }
                                        else
                                        {
                                            WriteData.SendData(TestActionHelp.GetWaitTime(driver, name, time));
                                        }
                                    }
                                }
                                else
                                {
                                    Boolean fl = true;
                                    n.Click();
                                    while (fl)
                                    {
                                        try
                                        {
                                            LocateElement(driver, waitForXPath, waitForClassName, waitForTitle, waitForTag, waitForInTag, waitForInLabel, waitForInText, waitForInValue, waitForId);
                                            fl = false;
                                        }
                                        catch (Exception ex) { }
                                    }
                                    if (sendInLog!=null)
                                    {
                                        Program.WriteLog("[CHANGED] " + driver.FindElements(By.TagName("span")).Where(i => i.Text.Equals(i.GetAttribute("title"))).First().Text);
                                    }
                                }
                                break;
                            }
                        case "switchTo":
                            {
                                if (text != "parent")
                                {
                                    IWebElement n = LocateElement(driver, objectXPath, objectClassName, objectTitle, objectTag, objectInTag, objectInLabel, objectInText, objectInValue, objectId);
                                    driver.SwitchTo().Frame(n);
                                }
                                else
                                {
                                    driver.SwitchTo().ParentFrame();
                                }
                                break;
                            }
                        case "sendKeys":
                            {
                                string buf = text;
                                Random r = new Random();
                                IWebElement n = LocateElement(driver, objectXPath, objectClassName, objectTitle, objectTag, objectInTag, objectInLabel, objectInText, objectInValue, objectId);
                                if (text.Contains("randomNumb"))
                                {
                                    buf = text.Replace("randomNumb", r.Next(1000000, 9999999).ToString());
                                }
                                if (text.Contains("randomStr"))
                                {
                                    buf = text.Replace("randomStr", Path.GetRandomFileName().Replace(".",""));
                                }
                                if (text.Contains("date"))
                                {
                                    buf = text.Replace("date", DateTime.Now.ToString("yyyyMMddHHmmss"));
                                }
                                if (text.Contains("FromFile"))
                                {
                                    string[] st = text.Split('|');
                                    List<string> list = File.ReadAllLines(st[1]).ToList();
                                    buf = list[r.Next(list.Count)];
                                }
                                if(text.Contains("user"))
                                {
                                    int c = Convert.ToInt32(File.ReadAllText(@"D:\Perfomance\ClientSide\Client_Side_Performance\Configs\count"));
                                    buf = text.Replace("user", String.Format("PERFTEST_{0:000}", c));
                                    c++;
                                    File.WriteAllText(@"D:\Perfomance\ClientSide\Client_Side_Performance\Configs\count", c.ToString());
                                }
                                
                                Thread.Sleep(1000);
                                

                                string s = n.GetAttribute("value");
                                string s1 = "";
                                for (int i = 0; i < s.Length;i++ )
                                {
                                    s1 += "\b";
                                }
                                n.SendKeys(s1);
                                n.SendKeys(buf);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.Message);
                Program.WriteLog(this.type + " " + this.name);
            }
        }
    }
}
