namespace L1a.Code.Interfaces
{
    public interface IDataMonitor<T>
    {
        bool IsEmpty { get; }

        bool IsFull { get; }

        void AddItem(T item);

        T RemoveItem();
    }
}
