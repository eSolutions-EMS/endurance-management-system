namespace NTS.Domain.Core.StaticOptions.Regional.Base;

public abstract class RegionalConfiguration : IRegionalConfiguration
{
    protected RegionalConfiguration(string countryIsoCode)
    {
        CountryIsoCode = countryIsoCode;
    }

    public string CountryIsoCode { get; set; }
    public bool ShouldOnlyUseAverageLoopSpeed { get; set; }
}
