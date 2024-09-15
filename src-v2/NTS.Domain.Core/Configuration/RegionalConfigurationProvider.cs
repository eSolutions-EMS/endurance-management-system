using NTS.Domain.Core.Configuration.Regions;

namespace NTS.Domain.Core.Configuration;

internal class RegionalConfigurationProvider
{
    static readonly RegionalConfiguration[] _configurations = [ new BulgarianConfiguration() ];

    public static IRegionalConfiguration? Get(Country? country)
    {
        return _configurations.FirstOrDefault(x => x.CountryIsoCode == country?.IsoCode);
    }
}
