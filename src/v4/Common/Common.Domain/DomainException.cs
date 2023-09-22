using System.Dynamic;

namespace Common.Domain;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, params string[] args) : base(message)
    {
        this.Args = args;
    }

    public string[] Args { get; } = Array.Empty<string>();
}
