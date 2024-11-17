using NTS.Domain.Core.StaticOptions.Regional;
using NTS.Domain.Core.StaticOptions.Regional.Base;

namespace NTS.Domain.Core.StaticOptions;

internal class RegionalConfigurationProvider
{
    public static IRegionalConfiguration? Get(Country? country)
    {
        return _configurations.FirstOrDefault(x => x.CountryIsoCode == country?.IsoCode);
    }

    static RegionalConfiguration[] _configurations = [new BulgarianConfiguration()];
}
