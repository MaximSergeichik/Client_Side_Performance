using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Web;


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
