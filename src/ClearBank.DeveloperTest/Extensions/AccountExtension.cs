using ClearBank.DeveloperTest.Types.Enums;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Exceptions;

namespace ClearBank.DeveloperTest.Extensions;

internal static class AccountExtension
{
    internal static bool ProcessPayment(this Account account, PaymentScheme scheme, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(scheme);
        ArgumentNullException.ThrowIfNull(amount);

        if (!account.ValidatePaymentScheme(scheme))
        {
            return false;
        }

        return scheme switch
        {
            PaymentScheme.Bacs => account.ProcessBacsPayment(amount),

            PaymentScheme.FasterPayments => account.ProcessFasterPayment(amount),

            PaymentScheme.Chaps => account.ProcessChapsPayment(amount),

            _ => throw new InvalidPaymentException()
        };
    }

    internal static bool ValidatePaymentScheme(this Account account, PaymentScheme scheme) => scheme switch
    {
        PaymentScheme.Bacs => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs),
        PaymentScheme.Chaps => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps),
        PaymentScheme.FasterPayments => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments),
        _ => throw new InvalidPaymentSchemeException()
    };

    internal static bool ProcessBacsPayment(this Account account, decimal amount)
    {
        account.Balance -= amount;
        return true;
    }

    internal static bool ProcessFasterPayment(this Account account, decimal amount)
    {
        if (account.Balance < amount)
        {
            return false;
        }
        account.Balance -= amount;
        return true;
    }

    internal static bool ProcessChapsPayment(this Account account, decimal amount)
    {
        if (account.Status != AccountStatus.Live)
        {
            return false;
        }
        account.Balance -= amount;
        return true;
    }
}