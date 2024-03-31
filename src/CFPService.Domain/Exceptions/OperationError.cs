using System.Runtime.Serialization;

namespace CFPService.Domain.Exceptions;

internal sealed class OperationError : Exception
{
    public OperationError()
    {
    }

    public OperationError(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public OperationError(string? message) : base(message)
    {
    }

    public OperationError(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}