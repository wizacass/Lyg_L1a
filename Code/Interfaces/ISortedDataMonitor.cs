namespace L1a.Code.Interfaces
{
    public interface ISortedDataMonitor<T>
    {
        void AddItemSorted(T item);

        T[] getItems();
    }
}
