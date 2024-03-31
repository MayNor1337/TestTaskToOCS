using System.Runtime.Serialization;

namespace CFPService.Domain.Exceptions;

public sealed class OperationException : Exception
{
    public OperationException()
    {
    }

    public OperationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public OperationException(string? message) : base(message)
    {
    }

    public OperationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}