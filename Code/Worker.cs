using System;
using L1a.Code.Interfaces;

namespace L1a.code
{
    public class Worker<T> where T : IComputable
    {
        private decimal _threshold;
        private readonly IDataMonitor<T> _dataMonitor;
        //private readonly ISortedResultMonitor<T> _resultMonitor;
        public int _counter = 0;

        public Worker(IDataMonitor<T> dataMonitor, /*ISortedResultMonitor<T> resultMonitor, */decimal threshold)
        {
            _dataMonitor = dataMonitor;
            //_resultMonitor = resultMonitor;
            _threshold = threshold;
        }

        public void Work()
        {
            while (!_dataMonitor.IsEmpty)
            {
                try
                {
                    var item = _dataMonitor.RemoveItem();
                    System.Console.WriteLine($"Value: {item.ComputedValue()}");
                    _counter++;
                    if (item.ComputedValue() <= _threshold)
                    {
                        //System.Console.WriteLine("Adding item to results monitor!");
                        //_resultMonitor.AddItemSorted(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //throw;
                }
            }
            System.Console.WriteLine($"Done! Processed {_counter} items.");
        }
    }
}
