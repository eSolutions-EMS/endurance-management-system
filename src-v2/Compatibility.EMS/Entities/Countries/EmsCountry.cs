using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Countries;

public class EmsCountry : EmsDomainBase<EmsCountryException>
{
    private EmsCountry() {}
    public EmsCountry(string isoCode, string name, int id) : base(GENERATE_ID)
    {
        this.Id = id;
        this.IsoCode = this.Validator.IsRequired(isoCode, nameof(isoCode));
        this.Name = this.Validator.IsRequired(name, nameof(name));
    }

    public string IsoCode { get; private set; }
    public string Name { get; private set; }
}
