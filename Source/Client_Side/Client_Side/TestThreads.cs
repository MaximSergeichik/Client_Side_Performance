using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        public TestThreads(int threadsCount, List<TestAction> plan, string Browser)
        {
            ThreadsCount = threadsCount;
            Threads = new List<TestThread>();
            for(int i=0;i<ThreadsCount;i++)
            {
                string name = String.Format("TestThread{0}", i);
                TestThread thread = new TestThread(name, plan, Browser);
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

        public int GetCountOfAbortedThreads()
        {
            int i = 0;
            foreach(var a in Threads)
            {
                if ((a.GetThreadState == ThreadState.Aborted.ToString()) || (a.GetThreadState == ThreadState.Stopped.ToString()))
                {
                    i++;
                }
            }
            return i;
        }

        #endregion
    }
}
