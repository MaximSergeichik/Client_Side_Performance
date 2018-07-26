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

        private List<TestAction> Plan;
        private string ThreadName;
        private int IterationsCount;
        private Thread th;

        #endregion

        #region Properties

        public List<TestAction> SetPlan
        {
            set
            {
                Plan = value;
            }
        }

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

        public TestThread(string ThreadName)
        {
            IterationsCount = 0;
            this.ThreadName = ThreadName;
            th = new Thread(() => RunTest());
            th.Name = this.ThreadName;
        }

        public void Start()
        {
            th.Start();
        }
        
        public void Abort()
        {
            th.Abort();
        }

        public void RunTest()
        {
            try
            {
            Start:
                if ((!TestActionHelp.GetCheckDuration && IterationsCount != TestActionHelp.GetIteration) || (TestActionHelp.GetCheckDuration))
                {
                    Plan.ForEach(j => j.PerformAction());
                    IterationsCount += 1;
                    File.WriteAllText(Directory.GetCurrentDirectory() + "/temp/" + ThreadName, IterationsCount.ToString());
                    goto Start;
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine(ex);
                Program.WriteLog(String.Format("Thread {0} was aboarted due the following error:\n{1}\nStackTrace:\n\t{2}", ThreadName, ex.Message, ex.StackTrace));
                WebDriver.Close();
                File.Delete(Directory.GetCurrentDirectory() + "/temp/" + ThreadName);
            }
        }

        #endregion
    }
}
