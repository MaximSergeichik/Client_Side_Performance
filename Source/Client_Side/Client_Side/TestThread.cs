﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using OpenQA.Selenium;

namespace Client_Side
{
    class TestThread
    {
        #region Data

        private TestPlan Test;
        private string ThreadName;
        private int IterationsCount;
        private Thread th;
        private string Browser;
        private IWebDriver driver;

        #endregion
        
        #region Properties
        
        public int GetIterationsCount
        {
            get
            {
                return IterationsCount;
            }
        }

        public string GetThreadName
        {
            get
            {
                return ThreadName;
            }
        }

        public string GetThreadState
        {
            get
            {
                return th.ThreadState.ToString();
            }
        }

        #endregion

        #region Methods

        public TestThread(string ThreadName, List<TestAction> TestPlan, string browser)
        {
            IterationsCount = 0;
            this.ThreadName = ThreadName;
            th = new Thread(() => RunTest());
            th.Priority = ThreadPriority.Highest;
            th.Name = this.ThreadName;
            Test = new TestPlan(TestPlan);
            Browser = browser;
        }

        public void Start()
        {
            th.Start();
        }
        
        public void Abort()
        {
            Test.StopTest();
            th.Abort();
        }

        public void RunTest()
        {
        Start:
            try
            {
                if (TestActionHelp.GetCheckDuration)
                {
                    driver = Test.InitiateWebDriver(Browser);
                    Test.ExecuteTest(driver);
                    File.WriteAllText(Directory.GetCurrentDirectory() + "/temp/" + ThreadName, IterationsCount.ToString());
                    IterationsCount += 1;
                    Program.ShowMessageInConsole(String.Format("Thread {0} iteration {1} was finished", th.Name, IterationsCount));
                    goto Start;
                }
                else if (!TestActionHelp.GetCheckDuration && IterationsCount != TestActionHelp.GetIteration)
                {
                    driver = Test.InitiateWebDriver(Browser);
                    Test.ExecuteTest(driver);
                    IterationsCount += 1;
                    File.WriteAllText(Directory.GetCurrentDirectory() + "/temp/" + ThreadName, IterationsCount.ToString());
                    Program.ShowMessageInConsole(String.Format("Thread {0} iteration {1} was finished", th.Name, IterationsCount));
                    goto Start;
                }
                else
                {
                    this.Abort();
                }
            }
            catch (ThreadAbortException ex)
            {
                Logger.WriteLog(String.Format("Thread {0} was aboarted due the following error:\n{1}\nStackTrace:\n\t{2}", ThreadName, ex.Message, ex.StackTrace), "error");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "error");
                Logger.WriteLog(ex.StackTrace, "error");
                goto Start;
            }

        }

        #endregion
    }
}
