using System;
using L1a.Code.Interfaces;

namespace L1a.Code
{
    public class SortedDataMonitor<T> : ISortedDataMonitor<T> where T : IComparable
    {
        private int _count;
        private readonly int _capacity;
        private readonly T[] _items;

        private static readonly object Lock = new object();

        public SortedDataMonitor(int count)
        {
            _items = new T[count];
            _capacity = count;
        }

        public void AddItemSorted(T item)
        {
            lock (Lock)
            {
                if (_count == _capacity) throw new Exception("Sorted data monitor array is full!");
                if (_count == 0 || item.CompareTo(_items[_count - 1]) >= 0)
                {
                    _items[_count++] = item;
                    return;
                }

                for (int i = 0; i < _count; i++)
                {
                    if (item.CompareTo(_items[i]) < 0)
                    {
                        for (int j = _count - 1; j >= i; j--)
                        {
                            _items[j + 1] = _items[j];
                        }

                        _items[i] = item;
                        _count++;
                        break;
                    }
                }
            }
        }

        public T[] GetItems()
        {
            lock (Lock)
            {
                var items = new T[_count];
                for (int i = 0; i < _count; i++)
                {
                    items[i] = _items[i];
                }

                return items;
            }
        }
    }
}
