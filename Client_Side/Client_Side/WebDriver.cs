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
    //Singleton implementation for IWebDriver

    class WebDriver
    {
        private WebDriver() { }

        private static IWebDriver driver;

        public static string browser;

        public static IWebDriver Driver
        {
            get
            {
                if (driver==null)
                {
                    switch(browser)
                    {
                        case "chrome":
                            {
                                ChromeOptions options = new ChromeOptions();
                                options.AddArgument("--start-maximized");

                                options.AddUserProfilePreference("cacheDisabled", true);

                                driver = new ChromeDriver(options);
                                driver.Manage().Cookies.DeleteAllCookies();
                                break;
                            }
                        case "firefox":
                            {
                                driver = new FirefoxDriver();
                                driver.Manage().Cookies.DeleteAllCookies();
                                break;
                            }
                    }
                }
                return driver;
            }
        }

        public static void Close()
        {
            driver.Close();
            driver = null;
        }
    }
}
