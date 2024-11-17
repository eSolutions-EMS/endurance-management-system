using NTS.Domain.Core.StaticOptions.Regional;
using NTS.Domain.Core.StaticOptions.Regional.Base;

namespace NTS.Domain.Core.StaticOptions;

internal class RegionalOptionProvider
{
    public static IRegionalOption? Get(Country? country)
    {
        return _configurations.FirstOrDefault(x => x.CountryIsoCode == country?.IsoCode);
    }

    static RegionalOption[] _configurations = [new BulgariaOption()];
}
