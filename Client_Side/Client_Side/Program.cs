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
        static int all_count;

        public static void ShowMessage(string s)
        {
            Console.WriteLine(s);
        }

        public static void WriteLog(string s)
        {
            string path = WriteData.GetPathToFile;
            File.AppendAllText(path+"/ClientSideLog.txt", s + "\n");
        }

        public static void RunTest(List<TestAction> plan)
        {
            try
            {
                Start:
                plan.ForEach(j => j.Perform());
                all_count += 1;
                File.WriteAllText(Directory.GetCurrentDirectory() + "/temp", all_count.ToString());
                goto Start;
            }
            catch(ThreadAbortException ex)
            {
                WebDriver.Close();
                File.Delete(Directory.GetCurrentDirectory() + "/temp");
            }
        }
        
        public static int numb;

        static int Main(string[] args)
        {
            all_count = 0;
            bool flag = true;
            try
            {
                //string path = args[0];
                string path = @"D:\Perfomance\ClientSide\Client_Side_Performance\Configs\config_C4C.properties";
                ShowMessage(String.Format("Start work with {0} config file", path));

                try
                {
                    //string PathToLogs = args[2];
                    string PathToLogs = @"D:\Perfomance\ClientSide\Client_Side_Performance\Results\";
                    WriteData.SetPathToFile = PathToLogs;
                }
                catch(Exception ex)
                {
                    flag = false;
                    ShowMessage(String.Format("Unable to parse folder for logs. Please, read help. \nClient_Side.exe --help"));
                }
                

                try
                {
                    string browser = "chrome";
                    //string browser = args[1];
                    WebDriver.browser = browser;
                }
                catch (Exception ex)
                {
                    ShowMessage(String.Format("Unable to parse browser for work. Please, read help. \nClient_Side.exe --help"));
                    flag = false;
                }
                try
                {
                    //string s = args[2];
                    string s = "true";
                    TestAction.isDebug = Convert.ToBoolean(s);
                    if (TestAction.isDebug)
                    {
                        ShowMessage(String.Format("Test will be executed with think times"));
                    }
                }
                catch (Exception ex) { }

                Program.numb = 0;

                if (ParseConfigs.ParseConfigFile(path) && flag)
                {
                    List<TestAction> plan = ParsePlan.Plan();
                    Console.WriteLine("Test Plan Parsed");


                    try
                    {
                        File.Delete(Directory.GetCurrentDirectory() + "/temp");
                    }
                    catch (Exception ex) { }

                    Thread th = new Thread(() => RunTest(plan));
                    th.Name = "TestThread";
                    th.Start();

                    while (th.ThreadState != ThreadState.Aborted)
                    {
                        if (TestActionHelp.GetDur)
                        {
                            if (DateTime.Now >= TestActionHelp.GetEndTime)
                            {
                                th.Abort();
                            }
                        }
                        else
                        {
                            try
                            {
                                if (Int32.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "/temp")) >= TestActionHelp.GetIteration)
                                {
                                    th.Abort();
                                }
                            }
                            catch (Exception ex)
                            { }
                        }
                    }

                }
                ShowMessage("Test was ended.");
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

