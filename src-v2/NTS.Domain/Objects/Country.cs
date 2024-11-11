using Not.Domain.Summary;

namespace NTS.Domain.Objects;

public record Country : DomainObject, ISummarizable
{
    public Country(string isoCode, string nfCode, string name)
    {
        IsoCode = isoCode;
        NfCode = nfCode;
        Name = name;
    }

    public string IsoCode { get; }
    public string NfCode { get; }
    public string Name { get; }

    public string Summarize()
    {
        return $"{Name}, {IsoCode}";
    }

    public override string ToString()
    {
        return Name;
    }
}
