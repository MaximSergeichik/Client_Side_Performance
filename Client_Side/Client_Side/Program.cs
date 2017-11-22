using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Client_Side
{
    class Program
    {
        public static void ShowMessage(string s)
        {
            Console.WriteLine(s);
        }

        public static void WriteLog(string s)
        {
            File.AppendAllText(@"D:/ClientSideLog.txt", s + "\n");
        }

        static int Main(string[] args)
        {
            try
            {
                string path = args[0];
                ShowMessage(String.Format("Start work with {0} config file", path));

                try
                {
                    string browser = args[1];
                    WebDriver.browser = browser;
                }
                catch (Exception ex)
                {
                    ShowMessage(String.Format("Unable to parse browser for work. Please, read help. \nClient_Side.exe --help"));
                }

                if (ParseConfigs.ParseConfigFile(path))
                {
                    List<TestAction> plan = ParsePlan.Plan();
                    Console.WriteLine("!!!TestPlan Start!!!");
                    plan.ForEach(i => i.Show());
                    Console.WriteLine("!!!!TestPlan End!!!!");
                    int count = 0;
                    bool flag = true;
                    while (flag)
                    {
                        count++;
                        Program.ShowMessage(count.ToString());
                        try
                        {
                            plan.ForEach(j => j.Perform());
                        }
                        catch (Exception ex)
                        {
                            Program.WriteLog(ex.Message);
                        }
                        WebDriver.Close();
                        if (TestActionHelp.GetDur)
                        {
                            DateTime end = TestActionHelp.GetEndTime;
                            DateTime now = DateTime.Now;
                            if (end < now)
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            if (count == TestActionHelp.GetIteration)
                            {
                                flag = false;
                            }
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                WebDriver.Driver.Close();
                Console.WriteLine(ex);
                Console.WriteLine("[ERROR]\tMiss command line argument with config file path!");
                return 1;
            }
        }
    }
}

