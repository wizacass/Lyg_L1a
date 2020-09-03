namespace L1a.Interfaces
{
    public interface IDataMonitor<T>
    {
        void AddItem(T item);

        T removeItem();
    }
}
