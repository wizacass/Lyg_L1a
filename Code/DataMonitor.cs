using System;
using System.Threading;
using L1a.Code.Interfaces;

namespace L1a.Code
{
    public class DataMonitor<T> : IDataMonitor<T>
    {
        private bool _isFinal = false;
        private int _count = 0;
        private readonly T[] _items;

        private static readonly object _lock = new object();

        public DataMonitor(int count)
        {
            _items = new T[count];
            _count = 0;
        }

        public bool IsEmpty
        {
            get
            {
                lock (_lock)
                {
                    return _count == 0;
                }
            }
        }

        public bool IsFull
        {
            get
            {
                lock (_lock)
                {
                    return _count == _items.Length;
                }
            }
        }

        public bool IsFinal
        {
            get
            {
                lock (_lock)
                {
                    return _isFinal;
                }
            }

            set
            {
                lock (_lock)
                {
                    _isFinal = value;
                    Monitor.PulseAll(_lock);
                }
            }
        }

        public void AddItem(T item)
        {
            lock (_lock)
            {
                while (IsFull) Monitor.Wait(_lock);
                _items[_count++] = item;
                Monitor.PulseAll(_lock);
            }
        }

        public T RemoveItem()
        {
            lock (_lock)
            {
                while (IsEmpty && !IsFinal) Monitor.Wait(_lock);
                if (IsEmpty && IsFinal) throw new Exception("Data monitor is empty!");
                var item = _items[--_count];
                Monitor.PulseAll(_lock);
                return item;
            }
        }
    }
}
