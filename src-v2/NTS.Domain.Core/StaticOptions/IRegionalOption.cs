namespace NTS.Domain.Core.StaticOptions;

public interface IRegionalOption
{
    string CountryIsoCode { get; }
    bool ShouldOnlyUseAverageLoopSpeed { get; }
}
