using Not.Exceptions;

namespace Not.Domain;

//TODO: Create AggregateDomainException and modify domains to batch their validation exceptions before throwing
public class DomainException : DomainExceptionBase
{
    public DomainException(string property, string message) : base(message)
    {
        Property = property;
    }
    public DomainException(string message) : base(message)
    {
    }

    public string? Property { get; }
}
