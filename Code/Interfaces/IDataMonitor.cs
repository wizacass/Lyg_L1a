namespace L1a.Code.Interfaces
{
    public interface IDataMonitor<T>
    {
        bool IsEmpty { get; }

        bool IsFull { get; }

        bool IsFinal { get; set; }

        void AddItem(T item);

        T RemoveItem();
    }
}
