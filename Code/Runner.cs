using System;
using System.Threading;
using L1a.Code.Interfaces;
using L1a.Models;

namespace L1a.code
{
    public class Runner<T> where T : IComputable
    {
        private readonly Thread[] _threads;

        public Runner(int count)
        {
            _threads = new Thread[count];
        }

        public void PrepareThreads(IDataMonitor<T> dataMonitor, ISortedDataMonitor<T> resultsMonitor, decimal threshold)
        {
            for (int i = 0; i < _threads.Length; i++)
            {
                var worker = new Worker<T>(dataMonitor, resultsMonitor, threshold);
                _threads[i] = new Thread(worker.Work);
            }
        }

        public void StartThreads()
        {
            foreach (var t in _threads)
            {
                t.Start();
            }
        }

        public void FinishThreads()
        {
            foreach (var t in _threads)
            {
                t.Join();
            }
        }
    }
}
