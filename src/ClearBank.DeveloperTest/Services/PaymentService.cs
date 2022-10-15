using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class PaymentService : IPaymentService
{
    public PaymentService(IDataStore<Account> dataStore)
    {
        DataStore = dataStore;
    }

    private IDataStore<Account> DataStore { get; }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        try
        {
            if (!DataStore.TryGet(request.DebtorAccountNumber, out var account))
            {
                return MakePaymentResult.Failed;
            }

            if (!account.ProcessPayment(request.PaymentScheme, request.Amount))
            {
                return MakePaymentResult.Failed;
            }

            DataStore.Update(account);
            return MakePaymentResult.Succeeded;
        }
        catch (Exception)
        {
            return MakePaymentResult.Failed;
        }
    }
}