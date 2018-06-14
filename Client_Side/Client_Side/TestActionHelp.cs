using System;
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

        private static bool Dur = true;

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

        public static List<string> GetTimes(IWebDriver driver, string Name)
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

        public static List<string> GetWaitTime(IWebDriver driver, string Name, int time)
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

        public static void WaitForElement(IWebDriver driver, TestAction action)
        {
            IWebElement n = null;
            while (n==null)
            {
                n = LocateElement(driver, action, true);
            }
        }

        public static IWebElement LocateElement(IWebDriver driver, TestAction action, Boolean wait)
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

            if (wait)
            {
                tag = action.waitForInTag;
                id = action.waitForId;
                inText = action.waitForInText;
                inTag = action.waitForInTag;
                inValue = action.waitForInValue;
                inLabel = action.waitForInLabel;
                title = action.waitForTitle;
                className = action.waitForClassName;
                xPath = action.waitForXPath;
            }
            else
            {
                tag = action.objectInTag;
                id = action.objectId;
                inText = action.objectInText;
                inTag = action.objectInTag;
                inValue = action.objectInValue;
                inLabel = action.objectInLabel;
                title = action.objectTitle;
                className = action.objectClassName;
                xPath = action.objectXPath;
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
                        n = driver.FindElements(By.TagName(tag)).Where(i => i.Text.Contains(inText)).First();
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
                if (inTag != null && title == null)
                {
                    n = driver.FindElements(By.ClassName(className)).Where(i => i.FindElements(By.XPath(inTag)).Count > 0).First().FindElement(By.XPath(inTag));
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
                Program.ShowMessage("[INFO]\t Test will be ended at " + endOfTheTest.ToString());
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
                Dur = false;
            }
        }

        public static string SetLocation
        {
            set
            {
                location = value;
            }
        }

        public static bool GetDur
        {
            get
            {
                return Dur;
            }
        }

        #endregion
    }
}
