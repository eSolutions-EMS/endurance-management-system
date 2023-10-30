using System.Dynamic;
using static Common.Localization.Localizer;

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

    public DomainException(params object[] args) : base(Localize(args))
    {
    }

    public string[] Args { get; } = Array.Empty<string>();
}
