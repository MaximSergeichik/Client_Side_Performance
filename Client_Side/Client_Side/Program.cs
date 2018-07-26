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
        public static void ShowMessageInConsole(string s)
        {
            Console.WriteLine(s);
        }

        public static void WriteLog(string s)
        {
            string path = WriteData.GetPathToFile;
            File.AppendAllText(path+"/ClientSideLog.txt", s + "\n");
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

        static int Main(string[] args)
        {
            //by default flag is in true
            //if tool receive some exception on the step of check of command line arguments flag will be set in false and test will be not started
            bool flag = true;
            try
            {
                //path to config file
                //string path = args[0];
                string path = @"D:\Perfomance\ClientSide\Client_Side_Performance\Configs\config_HM.properties";
                ShowMessageInConsole(String.Format("Start work with {0} config file", path));

                try
                {
                    //string PathToLogs = args[2];
                    string PathToLogs = @"D:\Perfomance\ClientSide\Client_Side_Performance\Results\";
                    WriteData.SetPathToFile = PathToLogs;
                }
                catch(Exception ex)
                {
                    flag = false;
                    ShowMessageInConsole(String.Format("Unable to parse folder for logs. Please, read help. \nClient_Side.exe --help"));
                    Console.WriteLine(ex);
                }
                

                try
                {
                    string browser = "chrome";
                    //string browser = args[1];
                    WebDriver.browser = browser;
                }
                catch (Exception ex)
                {
                    ShowMessageInConsole(String.Format("Unable to parse browser for work. Please, read help. \nClient_Side.exe --help"));
                    flag = false;
                    Console.WriteLine(ex);
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
                        Console.WriteLine(ex);
                    }

                    List<TestAction> Plan = ParsePlan.Plan();
                    Console.WriteLine("Test Plan Parsed");
                    
                    TestThreads Test = new TestThreads(TestActionHelp.GetThreadsCount, Plan);
                    if (TestActionHelp.GetCheckDuration)
                    {
                        Test.Start();
                        while (DateTime.Now<TestActionHelp.GetEndTime)
                        {
                            Thread.Sleep(60000);
                        }
                        Test.Abort();
                    }
                    else
                    {
                    notend:
                        Thread.Sleep(60000);
                        int countOfAbortedThreads = Test.GetStateOfAllThreads().Select(t => t.Value.Equals(ThreadState.Aborted)).Count();
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
                Console.ReadKey();
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

