namespace ClearBank.DeveloperTest.Types.Enums;

[Flags]
public enum AllowedPaymentSchemes : short
{
    None = 0,
    FasterPayments = 1 << 0,
    Bacs = 1 << 1,
    Chaps = 1 << 2
}