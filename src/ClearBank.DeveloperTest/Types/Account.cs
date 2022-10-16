using ClearBank.DeveloperTest.Types.Enums;

namespace ClearBank.DeveloperTest.Types;

public class Account
{
    public string AccountNumber { get; set; } = null!;
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }
}