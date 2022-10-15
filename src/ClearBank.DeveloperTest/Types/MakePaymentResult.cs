namespace ClearBank.DeveloperTest.Types;

public class MakePaymentResult
{
    private MakePaymentResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; }


    public static MakePaymentResult Failed { get; } = new MakePaymentResult(false);
    public static MakePaymentResult Succeeded { get; } = new MakePaymentResult(true);
}