using Not.Exceptions;

namespace NTS.Persistence.Exceptions;

public class ParentalViolationException : DomainExceptionBase
{
    public ParentalViolationException(string message) : base(message)
    {
    }
}
