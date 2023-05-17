namespace Core.Domain.Core.Exceptions;

public class DomainException : DomainExceptionBase
{
    public DomainException(string entity, string message, params object[] arguments)
        : this(entity, string.Format(message, arguments))
    {
    }
    public DomainException(string entity, string message)
    {
        this.Entity = entity;
        this.InitMessage = message;
    }

    protected override string Entity { get; }
}
