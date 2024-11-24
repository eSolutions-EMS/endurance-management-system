using Not.Domain.Base;

namespace NTS.Domain.Objects;

public record Country : DomainObject
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

    public override string ToString()
    {
        return Name;
    }
}
