namespace L1a.Interfaces
{
    public interface ISortedResultMonitor<T>
    {
        void AddItemSorted(T item);

        T[] getItems();
    }
}
