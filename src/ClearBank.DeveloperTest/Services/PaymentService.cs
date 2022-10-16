using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Extensions;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class PaymentService : IPaymentService
{
    private IDataStore<Account> DataStore { get; }

    public PaymentService(IDataStore<Account> dataStore)
    {
        DataStore = dataStore;
    }

    public MakePaymentResult MakePayment(MakePaymentRequest paymentRequest)
    {
        ArgumentNullException.ThrowIfNull(paymentRequest);

        try
        {
            if (!DataStore.TryGet(paymentRequest.DebtorAccountNumber, out var account))
            {
                return MakePaymentResult.Failed;
            }

            if (!account.ProcessPayment(paymentRequest.PaymentScheme, paymentRequest.Amount))
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