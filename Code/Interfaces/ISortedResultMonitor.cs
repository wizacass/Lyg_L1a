namespace L1a.Code.Interfaces
{
    public interface ISortedResultMonitor<T>
    {
        void AddItemSorted(T item);

        T[] getItems();
    }
}
