namespace NTS.Domain.Configuration;

public abstract class RegionalConfiguration : IRegionalConfiguration
{
    protected RegionalConfiguration(Country country)
    {
        Country = country;
    }

    public Country Country { get; set; }
    public bool AlwaysUseAverageLoopSpeed { get; set; }
}

public interface IRegionalConfiguration
{
    Country Country { get; }
    bool AlwaysUseAverageLoopSpeed { get; }
}