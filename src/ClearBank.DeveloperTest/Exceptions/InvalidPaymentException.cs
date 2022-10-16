// ReSharper disable UnusedMember.Global
namespace ClearBank.DeveloperTest.Exceptions;

[Serializable]
public class InvalidPaymentException : Exception
{
    public InvalidPaymentException()
    {
    }

    public InvalidPaymentException(string message)
        : base(message)
    {
    }

    public InvalidPaymentException(string message, Exception inner)
        : base(message, inner)
    {
    }
}