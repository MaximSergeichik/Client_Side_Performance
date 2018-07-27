using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Web;
using System.Text.RegularExpressions;


namespace Client_Side
{
    class TestActionHelp
    {

        #region Data
        
        enum Metrics
        {
            FullTime,
            ConnectTime,
            BackEnd,
            FrontEnd,
            DOM,
            RenderTime
        }

        private static string location;

        private static DateTime endOfTheTest;

        private static int Iteration;

        private static bool CheckDuration = true;

        private static int ThreadsCount;

        //This variable is created to store count of attempts to locate element using LocateElement method.
        //Exception will be generated only in case when 5 attempts were failed.
        private static int countOfExceptions;

        #endregion

        #region Methods

        static string Timestamp()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            return ((long)DateTime.UtcNow.Subtract(epoch).TotalMilliseconds).ToString();
        }

        static string GetValue(Metrics metric, IJavaScriptExecutor js)
        {
            if (metric == Metrics.FullTime)
            {
                return ((long)js.ExecuteScript("return window.performance.timing.loadEventEnd") - (long)js.ExecuteScript("return window.performance.timing.navigationStart")).ToString();
            }
            if (metric == Metrics.ConnectTime)
            {
                return ((long)js.ExecuteScript("return window.performance.timing.connectEnd") - (long)js.ExecuteScript("return window.performance.timing.connectStart")).ToString();
            }
            if (metric == Metrics.BackEnd)
            {
                return ((long)js.ExecuteScript("return window.performance.timing.responseEnd") - (long)js.ExecuteScript("return window.performance.timing.requestStart")).ToString();
            }
            if (metric == Metrics.DOM)
            {
                return ((long)js.ExecuteScript("return window.performance.timing.domContentLoadedEventEnd") - (long)js.ExecuteScript("return window.performance.timing.responseEnd")).ToString();
            }
            if (metric == Metrics.FrontEnd)
            {
                return ((long)js.ExecuteScript("return window.performance.timing.loadEventEnd") - (long)js.ExecuteScript("return window.performance.timing.responseEnd")).ToString();
            }
            if (metric == Metrics.RenderTime)
            {
                return ((long)js.ExecuteScript("return window.performance.timing.loadEventEnd") - (long)js.ExecuteScript("return window.performance.timing.domContentLoadedEventEnd")).ToString();
            }

            return "";
        }

        public static List<string> GetTimes(ref IWebDriver driver, string Name)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            List<string> result = new List<string>();

            string GetTimestamp = Timestamp();
            string url = HttpUtility.UrlEncode(driver.Url);

            result.Add(Name + ",metric=FullTime,location=" + location + " value=" + GetValue(Metrics.FullTime, js) + ",URL=\"" + url + "\" " + GetTimestamp);
            result.Add(Name + ",metric=ConnectTime,location=" + location + " value=" + GetValue(Metrics.ConnectTime, js) + ",URL=\"" + url + "\" " + GetTimestamp);
            result.Add(Name + ",metric=BackEnd,location=" + location + " value=" + GetValue(Metrics.BackEnd, js) + ",URL=\"" + url + "\" " + GetTimestamp);
            result.Add(Name + ",metric=FrontEnd,location=" + location + " value=" + GetValue(Metrics.FrontEnd, js) + ",URL=\"" + url + "\" " + GetTimestamp);
            result.Add(Name + ",metric=DOM,location=" + location + " value=" + GetValue(Metrics.DOM, js) + ",URL=\"" + url + "\" " + GetTimestamp);
            result.Add(Name + ",metric=RenderTime,location=" + location + " value=" + GetValue(Metrics.RenderTime, js) + ",URL=\"" + url + "\" " + GetTimestamp);


            return result;
        }

        public static List<string> GetWaitTime(ref IWebDriver driver, string Name, int time)
        {
            List<string> result = new List<string>();

            string url = HttpUtility.UrlEncode(driver.Url);
            result.Add("Client,transaction="+Name + ",metric=PageLoadTime,location=" + location + " value=" + time.ToString() + ",URL=\"" + url + "\" " + Timestamp());

            return result;
        }

        public static List<string> GetWaitTime(string Name, int time)
        {
            List<string> result = new List<string>();
            result.Add("Client,transaction=" + Name + ",metric=PageLoadTime,location=" + location + " value=" + time.ToString() + " " + Timestamp());

            return result;
        }

        public static void WaitForElement(ref IWebDriver driver, TestAction action)
        {
            IWebElement n = null;
            while (n==null)
            {
                n = LocateElement(ref driver, action, "1");
            }
        }

        //set type in "1" if you need to locate object for waiting
        //set type in "2" if you need to locate object for action
        //set type in "3" if you need to lcoate target object for moveTo command
        public static IWebElement LocateElement(ref IWebDriver driver, TestAction action, string type)
        {
            countOfExceptions = 0;

            while (countOfExceptions < 5)
            {
                try
                {
                    IWebElement n = null;

                    string tag;
                    string id;
                    string inText;
                    string inTag;
                    string inValue;
                    string inLabel;
                    string title;
                    string className;
                    string xPath;

                    if (type == "1")
                    {
                        tag = action.WaitForTag;
                        id = action.WaitForId;
                        inText = action.WaitForInText;
                        inTag = action.WaitForInTag;
                        inValue = action.WaitForInValue;
                        inLabel = action.WaitForInLabel;
                        title = action.WaitForTitle;
                        className = action.WaitForClassName;
                        xPath = action.WaitForXPath;
                    }
                    else if (type == "2")
                    {
                        tag = action.ObjectTag;
                        id = action.ObjectId;
                        inText = action.ObjectInText;
                        inTag = action.ObjectInTag;
                        inValue = action.ObjectInValue;
                        inLabel = action.ObjectInLabel;
                        title = action.ObjectTitle;
                        className = action.ObjectClassName;
                        xPath = action.ObjectXPath;
                    }
                    else
                    {
                        tag = action.TargetTag;
                        id = action.TargetId;
                        inText = action.TargetInText;
                        inTag = action.TargetInTag;
                        inValue = action.TargetInValue;
                        inLabel = action.TargetInLabel;
                        title = action.TargetTitle;
                        className = action.TargetClassName;
                        xPath = action.TargetXPath;
                    }

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
                            if (title.Contains("match"))
                            {
                                string pattern = title.Split('|').ToArray()[1];
                                Regex r = new Regex(pattern);

                                n = driver.FindElements(By.TagName(tag)).Where(i => r.IsMatch(i.GetAttribute("title"))).First();
                            }
                            else
                            {
                                List<IWebElement> elements = driver.FindElements(By.TagName(tag)).ToList();
                                List<string> list = new List<string>();
                                elements.ForEach(i => list.Add(i.GetAttribute("title")));
                                File.WriteAllLines(@"D:\test.te", list);
                                n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("title").Contains(title)).First();
                            }
                        }
                        if (title != null && inTag != null)
                        {
                            n = driver.FindElements(By.TagName(tag)).Where(i => i.GetAttribute("title").Contains(title)).First().FindElement(By.XPath(inTag));
                        }
                        if (inTag != null && inLabel != null)
                        {
                            List<IWebElement> list = driver.FindElements(By.TagName(tag)).ToList();
                            foreach (var a in list)
                            {
                                try
                                {
                                    if (a.FindElement(By.TagName("label")).Text.Contains(inLabel))
                                    {
                                        n = a;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(action.ToString());
                                    Console.WriteLine(ex);
                                }
                            }
                            n = n.FindElement(By.XPath(inTag));
                        }
                        if (inText != null)
                        {
                            if (!inText.Contains("match"))
                            {
                                List<IWebElement> elements = driver.FindElements(By.TagName(tag)).ToList();
                                n = elements.Where(i => i.GetAttribute("innerHTML").Contains(inText)).First();
                            }
                            else
                            {
                                string pattern = inText.Split('|').ToArray()[1];
                                Regex r = new Regex(pattern);


                                n = driver.FindElements(By.TagName(tag)).Where(i => r.IsMatch(i.Text)).First();
                            }
                        }
                    }
                    if (className != null)
                    {
                        if (inText != null && inTag == null)
                        {
                            n = driver.FindElements(By.ClassName(className)).Where(i => i.Text.Contains(inText)).First();
                            goto m;
                        }
                        if (inTag != null && title == null && inText == null)
                        {
                            List<IWebElement> elements = driver.FindElements(By.ClassName(className)).ToList();
                            n = elements.Where(i => i.FindElements(By.XPath(inTag)).Count > 0).First().FindElement(By.XPath(inTag));
                            goto m;
                        }
                        if (title != null && inTag == null)
                        {
                            n = driver.FindElements(By.ClassName(className)).Where(i => i.GetAttribute("title").Contains(title)).First();
                            goto m;
                        }
                        if (title != null && inTag != null)
                        {
                            n = driver.FindElements(By.ClassName(className)).Where(i => i.GetAttribute("title").Contains(title)).First().FindElement(By.XPath(inTag));
                            goto m;
                        }
                        if (inTag != null && inText != null)
                        {
                            if (inText.Contains("match"))
                            {
                                string pattern = inText.Split('|').ToArray()[1];
                                Regex r = new Regex(pattern);

                                n = driver.FindElements(By.ClassName(className)).Where(k => k.FindElements(By.XPath(inTag)).Where(j => r.IsMatch(j.Text)) != null).First();
                                goto m;
                            }
                            else
                            {
                                n = driver.FindElements(By.ClassName(className)).Where(k => k.FindElements(By.XPath(inTag)).Where(j => j.Text.Contains(inText)) != null).First();
                                goto m;
                            }

                        }
                    }

                m:
                    return n;
                }
                catch(Exception ex)
                {
                    if (countOfExceptions < 4)
                    {
                        countOfExceptions++;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            return null;
        }

        public static void MoveElementTo(ref IWebDriver driver, IWebElement source, IWebElement target)
        {
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(driver);            
            var mouse = ((IHasInputDevices) driver).Mouse;
            var loc = (ILocatable) target;
            action.ClickAndHold(source).MoveToElement(target).Perform();
            mouse.MouseUp(loc.Coordinates);
        }

        public static void SwitchTab(ref IWebDriver driver)
        {
            if (driver.WindowHandles.Count < 2)
            {
                Program.WriteLog("Unable to switch tab. Only one tab is opened in browser.");
            }
            else
            {
                string firstTabHandle = driver.CurrentWindowHandle;

                foreach (string handle in driver.WindowHandles)
                {
                    if (handle != firstTabHandle)
                    {
                        driver.SwitchTo().Window(handle);
                    }
                }
            }
        }

        #endregion

        #region Properties

        public static DateTime GetEndTime
        {
            get
            {
                return endOfTheTest;
            }
        }

        public static Int32 SetEndTime
        {
            set
            {
                DateTime now = DateTime.Now;
                endOfTheTest = now.AddMinutes(value);
                Program.ShowMessageInConsole("[INFO]\t Test will be ended at " + endOfTheTest.ToString());
            }
        }

        public static int GetIteration
        {
            get
            {
                return Iteration;
            }
        }

        public static int SetIteration
        {
            set
            {
                Iteration = value;
                CheckDuration = false;
            }
        }

        public static string SetLocation
        {
            set
            {
                location = value;
            }
        }

        public static bool GetCheckDuration
        {
            get
            {
                return CheckDuration;
            }
        }

        public static int GetThreadsCount
        {
            get
            {
                return ThreadsCount;
            }
        }

        public static int SetThreadsCount
        {
            set
            {
                ThreadsCount = value;
            }
        }

        #endregion
    }
}
