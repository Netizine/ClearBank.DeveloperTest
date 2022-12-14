using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data;

public class AccountDataStore : IDataStore<Account>
{
    public bool TryGet(string accountNumber, out Account account)
    {
        // Access database to retrieve account, code removed for brevity
        account = new Account
        {
            AccountNumber = accountNumber
        };
        return true;
    }

    public void Update(Account account)
    {
        // Update account in database, code removed for brevity
    }
}