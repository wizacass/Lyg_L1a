using System;
using L1a.Code.Interfaces;

namespace L1a.Code
{
    public class DataMonitor<T> : IDataMonitor<T>
    {
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

        public void AddItem(T item)
        {
            if (IsFull) throw new Exception("Monitor is full!");
            lock (_lock)
            {
                if (IsFull) throw new Exception("Monitor is full!");
                _items[_count++] = item;
            }
        }

        public T RemoveItem()
        {
            if (IsEmpty) throw new Exception("Monitor is empty!");
            lock (_lock)
            {
                if (IsEmpty) throw new Exception("Monitor is empty!");
                return _items[--_count];
            }
        }
    }
}
