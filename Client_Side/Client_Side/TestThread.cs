using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Client_Side
{
    class TestThread
    {
        #region Data

        private TestPlan Test;
        private string ThreadName;
        private int IterationsCount;
        private Thread th;        

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
            th.Name = this.ThreadName;
            Test = new TestPlan(TestPlan);
            Test.SetBrowser = browser;
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
                if ((!TestActionHelp.GetCheckDuration && IterationsCount != TestActionHelp.GetIteration) || (TestActionHelp.GetCheckDuration))
                {
                    Test.ExecuteTest();
                    IterationsCount += 1;
                    File.WriteAllText(Directory.GetCurrentDirectory() + "/temp/" + ThreadName, IterationsCount.ToString());
                    goto Start;
                }
            }
            catch (Exception ex)
            {
                Program.ShowMessageInConsole(ex.ToString());
                Program.WriteLog(String.Format("Thread {0} was aboarted due the following error:\n{1}\nStackTrace:\n\t{2}", ThreadName, ex.Message, ex.StackTrace));
                goto Start;
            }

        }

        #endregion
    }
}
