// ReSharper disable UnusedMember.Global

using System.Runtime.Serialization;

namespace ClearBank.DeveloperTest.Exceptions;

[Serializable]
public class InvalidPaymentSchemeException : Exception, ISerializable
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