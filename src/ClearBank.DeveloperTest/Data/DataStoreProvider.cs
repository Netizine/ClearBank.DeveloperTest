using ClearBank.DeveloperTest.Data.Enums;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data;

internal class DataStoreProvider
{
    private readonly DataStoreType _dataStoreDataStoreType;

    public DataStoreProvider(DataStoreType dataStoreType)
    {
        _dataStoreDataStoreType = dataStoreType;
    }

    public DataStoreProvider(string dataStoreType)
        : this((DataStoreType)Enum.Parse(typeof(DataStoreType), dataStoreType))
    {

    }

    public IDataStore<Account> GetAccountDataStore()
    {
        return _dataStoreDataStoreType switch
        {
            DataStoreType.Backup => new BackupAccountDataStore(),
            _ => new AccountDataStore(),
        };
    }
}