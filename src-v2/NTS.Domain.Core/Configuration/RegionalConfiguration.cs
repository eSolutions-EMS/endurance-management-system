namespace NTS.Domain.Core.Configuration;

public abstract class RegionalConfiguration : IRegionalConfiguration
{
    protected RegionalConfiguration(string countryIsoCode)
    {
        CountryIsoCode = countryIsoCode;
    }

    public string CountryIsoCode { get; set; }
    public bool ShouldOnlyUseAverageLoopSpeed { get; set; }
}

public interface IRegionalConfiguration
{
    string CountryIsoCode { get; }
    bool ShouldOnlyUseAverageLoopSpeed { get; }
}
