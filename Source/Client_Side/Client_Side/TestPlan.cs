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

        public IWebDriver InitiateWebDriver(string Browser)
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
            return Driver;
        }

        public void ExecuteTest(IWebDriver driver)
        {
            Driver = driver;
            Plan.ForEach(action => action.PerformAction(ref Driver));
            Driver.Quit();
        }

        public void StopTest()
        {
            Driver.Quit();
        }

        #endregion

    }
}
