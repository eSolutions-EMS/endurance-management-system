namespace NTS.Domain.Core.StaticOptions;

public interface IRegionalConfiguration
{
    string CountryIsoCode { get; }
    bool ShouldOnlyUseAverageLoopSpeed { get; }
}
