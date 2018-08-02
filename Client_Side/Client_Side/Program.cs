using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Client_Side
{
    class Program
    {

        public static DateTime start_timestamp;

        public static void ShowMessageInConsole(string s)
        {
            Console.WriteLine(s);
            Logger.WriteLog(s, "info");
        }

        //public static void RunTest(List<TestAction> plan)
        //{
        //    try
        //    {
        //        Start:
        //        plan.ForEach(j => j.Perform());
        //        all_count += 1;
        //        File.WriteAllText(Directory.GetCurrentDirectory() + "/temp", all_count.ToString());
        //        goto Start;
        //    }
        //    catch(ThreadAbortException ex)
        //    {
        //        Console.WriteLine(ex);
        //        WebDriver.Close();
        //        File.Delete(Directory.GetCurrentDirectory() + "/temp");
        //    }
        //}

        static void Main(string[] args)
        {
            //by default flag is in true
            //if tool receive some exception on the step of check of command line arguments flag will be set in false and test will be not started
            bool flag = true;

            start_timestamp = DateTime.Now;
            try
            {
                //path to config file
                //string path = args[0];
                string path = @"D:\Perfomance\ClientSide\Client_Side_Performance\Configs\config_HM.properties";
                ShowMessageInConsole(String.Format("Start work with {0} config file", path));

                
                string browser = "chrome";
                try
                {
                    //string browser = args[1];
                }
                catch (Exception ex)
                {
                    ShowMessageInConsole(String.Format("Unable to parse browser for work. Please, read help. \nClient_Side.exe --help"));
                    flag = false;
                    ShowMessageInConsole(ex.ToString());
                    Logger.WriteLog(ex.Message, "error");
                    Logger.WriteLog(ex.StackTrace, "message");
                }

                if (ParseConfigs.ParseConfigFile(path) && flag)
                {
                    try
                    {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + "/temp"))
                        {
                            Directory.Delete(Directory.GetCurrentDirectory() + "/temp", true);
                        }
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/temp");
                    }
                    catch (Exception ex)
                    {
                        ShowMessageInConsole(ex.ToString());
                        Logger.WriteLog(ex.Message, "error");
                        Logger.WriteLog(ex.StackTrace, "error");

                    }

                    List<TestAction> Plan = ParsePlan.Plan();
                    ShowMessageInConsole("Test Plan Parsed");
                    Logger.WriteLog("Test Plan Parsed", "info");
                    
                    TestThreads testThreads = new TestThreads(TestActionHelp.GetThreadsCount, Plan, browser);
                    testThreads.Start();
                    if (TestActionHelp.GetCheckDuration)
                    {
                        while (DateTime.Now<TestActionHelp.GetEndTime)
                        {
                            Thread.Sleep(60000);
                        }
                        //Test.Abort();
                    }
                    else
                    {
                    notend:
                        Thread.Sleep(60000);
                        int countOfAbortedThreads = testThreads.GetStateOfAllThreads().Select(t => t.Value.Equals(ThreadState.Aborted)).Count();
                        if (countOfAbortedThreads == TestActionHelp.GetThreadsCount)
                        {
                            goto end;
                        }
                        else
                        {
                            goto notend;
                        }
                    }

                }
            end:
                ShowMessageInConsole("Test was ended.");
            }
            catch (Exception ex)
            {
                ShowMessageInConsole(ex.ToString());
                Logger.WriteLog(ex.Message, "error");
                Logger.WriteLog(ex.StackTrace, "error");
            }
        }
    }
}

