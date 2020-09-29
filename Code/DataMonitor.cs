using System;
using System.Threading;
using L1a.Code.Interfaces;

namespace L1a.Code
{
    public class DataMonitor<T> : IDataMonitor<T>
    {
        private bool _isFinal;
        private int _count;
        private readonly T[] _items;

        private static readonly object Lock = new object();

        public DataMonitor(int count)
        {
            _items = new T[count];
            _count = 0;
        }

        public bool IsEmpty
        {
            get
            {
                lock (Lock)
                {
                    return _count == 0;
                }
            }
        }

        public bool IsFull
        {
            get
            {
                lock (Lock)
                {
                    return _count == _items.Length;
                }
            }
        }

        public bool IsFinal
        {
            get
            {
                lock (Lock)
                {
                    return _isFinal;
                }
            }

            set
            {
                lock (Lock)
                {
                    _isFinal = value;
                    Monitor.PulseAll(Lock);
                }
            }
        }

        public void AddItem(T item)
        {
            lock (Lock)
            {
                while (IsFull) Monitor.Wait(Lock);
                _items[_count++] = item;
                Monitor.PulseAll(Lock);
            }
        }

        public T RemoveItem()
        {
            lock (Lock)
            {
                while (IsEmpty && !IsFinal) Monitor.Wait(Lock);
                if (IsEmpty && IsFinal) throw new Exception("Data monitor is empty!");
                var item = _items[--_count];
                Monitor.PulseAll(Lock);
                return item;
            }
        }
    }
}
