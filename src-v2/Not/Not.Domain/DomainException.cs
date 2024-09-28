using Not.Localization;
using System.Diagnostics.CodeAnalysis;

namespace Not.Domain;

//TODO: Create AggregateDomainException and modify domains to batch their validation exceptions before throwing
public class DomainException : Exception
{
    public DomainException(string property, string message) : base(message.Localize())
    {
        Property = property;
    }
    public DomainException(
        string property,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message,
        params string[] args) : this(message, args)
    {
        Property = property;
    }
    public DomainException(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message,
        params string[] args) : base(LocalizeAndFormat(message, args))
    {
    }
    public DomainException(string message) : base(message.Localize())
    {
    }

    public string? Property { get; }

    static string LocalizeAndFormat(string template, string[] args)
    {
        template = template.Localize();
        args = args.Localize().ToArray();
        return string.Format(template, args);
    }
}
