using System.Runtime.Serialization;

namespace CFPService.Domain.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException()
    {
    }

    public ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ValidationException(string? message) : base(message)
    {
    }

    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}