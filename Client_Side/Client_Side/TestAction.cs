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
        #region Data

        public static string Creditianals;

        public List<TestAction> InnerActions = new List<TestAction>();


        public string Type { get; set; }
        public string InnerNodeText { get; set; }
        public string UndefinedAttribute { get; set; }
        public string Name { get; set; }
        //this attribute is used to define the need of measurement for this action
        public string Measure { get; set; }
        public string MeasureType { get; set; }
        //this attribute is used to define the need of waiting for some object for this action
        public string Wait { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        //this attribute is used to define count of iterations in cycle
        public string Iterations { get; set; }

        //attributes for waiting of some element
        public string WaitForXPath { get; set; }
        public string WaitForClassName { get; set; }
        public string WaitForTitle { get; set; }
        public string WaitForTag { get; set; }
        public string WaitForInTag { get; set; }
        public string WaitForInLabel { get; set; }
        public string WaitForInText { get; set; }
        public string WaitForInValue { get; set; }
        public string WaitForId { get; set; }

        //attributes for detecting of actual element
        public string ObjectXPath { get; set; }
        public string ObjectClassName { get; set; }
        public string ObjectTitle { get; set; }
        public string ObjectTag { get; set; }
        public string ObjectInTag { get; set; }
        public string ObjectInLabel { get; set; }
        public string ObjectInText { get; set; }
        public string ObjectInValue { get; set; }
        public string ObjectId { get; set; }

        //attributes for detecting of the target element for moveTo command
        public string TargetXPath { get; set; }
        public string TargetClassName { get; set; }
        public string TargetTitle { get; set; }
        public string TargetTag { get; set; }
        public string TargetInTag { get; set; }
        public string TargetInLabel { get; set; }
        public string TargetInText { get; set; }
        public string TargetInValue { get; set; }
        public string TargetId { get; set; }

        #endregion

        #region Methods

        public TestAction() { }

       
        public void Show()
        {
            switch (Type)
            {
                case "navigate": { Program.ShowMessageInConsole(String.Format("Command: {0} URL: {1} Name: {2}", Type, ObjectXPath, Name)); break; }
                case "click": { Program.ShowMessageInConsole(String.Format("Command: {0} xPath: {1} IsNewPage: {2} Name: {3}", Type, ObjectXPath, UndefinedAttribute, Name)); break; }
                case "sendKeys": { Program.ShowMessageInConsole(String.Format("Command: {0} xPath: {1} string: {2}", Type, ObjectXPath, UndefinedAttribute)); break; }
            }
        }

        public void PerformAction()
        {
            try
            {
                IWebDriver driver = WebDriver.Driver;
                switch (Type)
                {
                    case "navigate":
                        {
                            if (Measure == "true")
                            {
                                switch (MeasureType)
                                {
                                    case "API":
                                        {
                                            driver.Navigate().GoToUrl(InnerNodeText);
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            WriteData.SendData(TestActionHelp.GetTimes(driver, Name));
                                            break;
                                        }
                                    case "Watch":
                                        {
                                            DateTime start = DateTime.Now;
                                            driver.Navigate().GoToUrl(InnerNodeText);
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            DateTime end = DateTime.Now;
                                            int time = (int)(end - start).TotalMilliseconds;
                                            WriteData.SendData(TestActionHelp.GetWaitTime(this.Name, time));
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                driver.Navigate().GoToUrl(ObjectXPath);
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            break;
                        }
                    case "click":
                        {
                            IWebElement el = TestActionHelp.LocateElement(driver, this, "2");
                            if (Measure == "true")
                            {
                                switch (MeasureType)
                                {
                                    case "API":
                                        {
                                            el.Click();
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            WriteData.SendData(TestActionHelp.GetTimes(driver, Name));
                                            break;
                                        }
                                    case "Watch":
                                        {
                                            DateTime start = DateTime.Now;
                                            el.Click();
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            DateTime end = DateTime.Now;
                                            int time = (int)(end - start).TotalMilliseconds;
                                            WriteData.SendData(TestActionHelp.GetWaitTime(this.Name, time));
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                el.Click();
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            break;
                        }
                    case "sendKeys":
                        {
                            IWebElement el = TestActionHelp.LocateElement(driver, this, "2");
                            if (Measure == "true")
                            {
                                switch (MeasureType)
                                {
                                    case "API":
                                        {
                                            el.SendKeys(Text);
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            WriteData.SendData(TestActionHelp.GetTimes(driver, Name));
                                            break;
                                        }
                                    case "Watch":
                                        {
                                            DateTime start = DateTime.Now;
                                            el.SendKeys(Text);
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            DateTime end = DateTime.Now;
                                            int time = (int)(end - start).TotalMilliseconds;
                                            WriteData.SendData(TestActionHelp.GetWaitTime(this.Name, time));
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                el.SendKeys(Text);
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            break;
                        }
                    case "refresh":
                        {
                            if (Measure == "true")
                            {
                                switch (MeasureType)
                                {
                                    case "API":
                                        {
                                            driver.Navigate().Refresh();
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            WriteData.SendData(TestActionHelp.GetTimes(driver, Name));
                                            break;
                                        }
                                    case "Watch":
                                        {
                                            DateTime start = DateTime.Now;
                                            driver.Navigate().Refresh();
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            DateTime end = DateTime.Now;
                                            int time = (int)(end - start).TotalMilliseconds;
                                            WriteData.SendData(TestActionHelp.GetWaitTime(this.Name, time));
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                driver.Navigate().Refresh();
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            break;
                        }
                    case "wait":
                        {
                            TestActionHelp.WaitForElement(driver, this);
                            break;
                        }
                    case "for":
                        {
                            int count = Int32.Parse(Iterations);
                            for (int i = 0; i < count; i++)
                            {
                                InnerActions.ForEach(k => k.PerformAction());
                                Console.WriteLine(i);
                            }
                            break;
                        }
                    case "if":
                        {
                            IWebElement n = TestActionHelp.LocateElement(driver, this, "2");
                            if (Value != null)
                            {
                                if (n.GetAttribute("value").Equals(Value))
                                {
                                    InnerActions.ForEach(i => i.PerformAction());
                                }
                            }
                            if (Text != null)
                            {
                                if (n.Text.Contains(Text))
                                {
                                    InnerActions.ForEach(i => i.PerformAction());
                                }
                            }
                            break;
                        }
                    case "thinkTime":
                        {
                            Thread.Sleep(Convert.ToInt32(InnerNodeText));
                            break;
                        }
                    case "switchTo":
                        {
                            if (Text != "parent")
                            {
                                IWebElement n = TestActionHelp.LocateElement(driver, this, "2");
                                driver.SwitchTo().Frame(n);
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            else
                            {
                                driver.SwitchTo().ParentFrame();
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            break;
                        }
                    case "moveTo":
                        {
                            IWebElement source = TestActionHelp.LocateElement(driver, this, "2");
                            IWebElement target = TestActionHelp.LocateElement(driver, this, "3");
                            if (Measure == "true")
                            {
                                switch (MeasureType)
                                {
                                    case "API":
                                        {
                                            TestActionHelp.MoveElementTo(driver, source, target);
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            WriteData.SendData(TestActionHelp.GetTimes(driver, Name));
                                            break;
                                        }
                                    case "Watch":
                                        {
                                            TestActionHelp.MoveElementTo(driver, source, target);
                                            DateTime start = DateTime.Now;                                            
                                            if (Wait == "true")
                                            {
                                                TestActionHelp.WaitForElement(driver, this);
                                            }
                                            DateTime end = DateTime.Now;
                                            int time = (int)(end - start).TotalMilliseconds;
                                            WriteData.SendData(TestActionHelp.GetWaitTime(this.Name, time));
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                TestActionHelp.MoveElementTo(driver, source, target);
                                if (Wait == "true")
                                {
                                    TestActionHelp.WaitForElement(driver, this);
                                }
                            }
                            break;
                        }
                    case "switchTab":
                        {
                            TestActionHelp.SwitchTab(driver);
                            if (Wait == "true")
                            {
                                TestActionHelp.WaitForElement(driver, this);
                            }
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.Message);
                Program.WriteLog(this.Type + " " + this.Name);
            }
        }

        #endregion
    }
}