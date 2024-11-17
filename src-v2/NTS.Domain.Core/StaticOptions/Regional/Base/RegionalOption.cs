namespace NTS.Domain.Core.StaticOptions.Regional.Base;

public abstract class RegionalOption : IRegionalOption
{
    protected RegionalOption(string countryIsoCode)
    {
        CountryIsoCode = countryIsoCode;
    }

    public string CountryIsoCode { get; set; }
    public bool ShouldOnlyUseAverageLoopSpeed { get; set; }
}
