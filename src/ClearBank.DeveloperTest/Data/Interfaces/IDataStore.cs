namespace ClearBank.DeveloperTest.Data.Interfaces;

public interface IDataStore<T>
{
    bool TryGet(string id, out T value);

    void Update(T item);
}