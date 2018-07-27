using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Client_Side
{
    class TestPlan
    {

        #region Data

        List<TestAction> Plan;
        IWebDriver Driver;
        string Browser;

        #endregion

        #region Methods

        public TestPlan(List<TestAction> plan)
        {
            Plan = plan;            
        }

        private void InitiateWebDriver()
        {
            switch (Browser)
            {
                case "chrome":
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.AddArgument("--start-maximized");

                        options.AddUserProfilePreference("cacheDisabled", true);

                        Driver = new ChromeDriver(options);
                        Driver.Manage().Cookies.DeleteAllCookies();
                        Driver.Manage().Window.Maximize();
                        break;
                    }
                case "firefox":
                    {
                        Driver = new FirefoxDriver();
                        Driver.Manage().Cookies.DeleteAllCookies();
                        break;
                    }
            }
        }

        public void ExecuteTest()
        {
            InitiateWebDriver();
            Plan.ForEach(action => action.PerformAction(ref Driver));
            Driver.Close();
        }

        public void StopTest()
        {
            Driver.Close();
        }

        #endregion

        #region Properties

        public string SetBrowser
        {
            set
            {
                Browser = value;
            }
        }

        #endregion

    }
}
