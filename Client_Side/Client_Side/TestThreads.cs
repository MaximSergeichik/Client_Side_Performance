using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Side
{
    class TestThreads
    {
        #region Data

        List<TestThread> Threads;
        int ThreadsCount;

        #endregion

        #region Properties

        public int GetThreadsCount
        {
            get
            {
                return ThreadsCount;
            }
        }

        #endregion

        #region Methods

        public TestThreads(int threadsCount, List<TestAction> plan)
        {
            ThreadsCount = threadsCount;
            for(int i=0;i<ThreadsCount;i++)
            {
                string name = String.Format("TestThread{0}", i);
                TestThread thread = new TestThread(name);
                thread.SetPlan = plan;
                Threads.Add(thread);
            }
        }

        public void Start()
        {
            Threads.ForEach(t => t.Start());
        }

        public void Abort()
        {
            Threads.ForEach(t => t.Abort());
        }

        public Dictionary<string, string> GetStateOfAllThreads()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Threads.ForEach(t => result.Add(t.GetThreadName, t.GetThreadState));
            return result;
        }

        #endregion
    }
}
