// ReSharper disable UnusedMember.Global
using System.Runtime.Serialization;

namespace ClearBank.DeveloperTest.Exceptions;

[Serializable]
public class InvalidPaymentException : Exception, ISerializable
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