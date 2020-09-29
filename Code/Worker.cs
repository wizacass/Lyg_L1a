using System;
using L1a.Code.Interfaces;

namespace L1a.code
{
    public class Worker<T> where T : IComputable
    {
        private readonly decimal _threshold;
        private readonly IDataMonitor<T> _dataMonitor;
        private readonly ISortedDataMonitor<T> _resultMonitor;
        private int _counter;

        public Worker(IDataMonitor<T> dataMonitor, ISortedDataMonitor<T> resultMonitor, decimal threshold)
        {
            _dataMonitor = dataMonitor;
            _resultMonitor = resultMonitor;
            _threshold = threshold;
        }

        public void Work()
        {
            while (true)
            {
                try
                {
                    var item = _dataMonitor.RemoveItem();
                    _counter++;
                    if (item.ComputedValue() <= _threshold)
                    {
                        _resultMonitor.AddItemSorted(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (_dataMonitor.IsEmpty && _dataMonitor.IsFinal)
                {
                    break;
                }
            }

            Console.WriteLine($"Done! Processed {_counter} items.");
        }
    }
}
