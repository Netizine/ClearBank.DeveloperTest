using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data;

public class BackupAccountDataStore : IDataStore<Account>
{
    public bool TryGet(string accountNumber, out Account account)
    {
        // Access backup data base to retrieve account, code removed for brevity and assuming success
        account = new Account();
        return true;
    }

    public void Update(Account account)
    {
        // Update account in backup database, code removed for brevity
    }
}