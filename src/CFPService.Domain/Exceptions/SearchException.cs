using System.Runtime.Serialization;

namespace CFPService.Domain.Exceptions;

public sealed class SearchException : Exception
{
    public SearchException()
    {
    }

    public SearchException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public SearchException(string? message) : base(message)
    {
    }

    public SearchException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}