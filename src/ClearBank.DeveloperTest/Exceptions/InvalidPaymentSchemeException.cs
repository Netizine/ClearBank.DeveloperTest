// ReSharper disable UnusedMember.Global
namespace ClearBank.DeveloperTest.Exceptions;

public class InvalidPaymentSchemeException : Exception
{
    public InvalidPaymentSchemeException()
    {
    }

    public InvalidPaymentSchemeException(string message)
        : base(message)
    {
    }

    public InvalidPaymentSchemeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}