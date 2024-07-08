namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsCountry : EmsDomainBase<EmsCountryException>, ICountryState
{
    private EmsCountry() { }
    public EmsCountry(string isoCode, string name, int id) : base(default)
    {
        Id = id;
        IsoCode = Validator.IsRequired(isoCode, nameof(isoCode));
        Name = Validator.IsRequired(name, nameof(name));
    }

    public string IsoCode { get; private set; }
    public string Name { get; private set; }
}
