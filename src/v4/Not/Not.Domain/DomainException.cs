using System.Diagnostics.CodeAnalysis;
using static Common.Localization.Localizer;

namespace Common.Domain;

//TODO: Create AggregateDomainException and modify domains to batch their validation exceptions before throwing
public class DomainException : Exception
{
    public DomainException(string message) : base(Localize(message))
    {
    }
    public DomainException(string property, string message) : base(Localize(message))
    {
        Property = property;
    }
    public DomainException(
        string property, 
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, 
        params string[] args)
        : base(string.Format(Localize(message), LocalizeSeparately(args).ToArray()))
    {
        Property = property;
    }
    public DomainException(string message, params string[] args) : base(message)
    {
        this.Args = args;
    }

    public DomainException(params object[] args) : base(Localize(args))
    {
    }

    public string[] Args { get; } = Array.Empty<string>();
    public string? Property { get; }
}
