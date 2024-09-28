using Not.Injection;
using NTS.Domain.Objects;
using System.Collections.ObjectModel;

namespace NTS.Judge.Blazor.Contexts;

public class CountriesContext : ICountriesContext
{
    public CountriesContext()
    {
        this.Countries = new(new List<Country>()
        {
            new Country("BG", "zz", "Bulgaria"),
            new Country("US", "zz", "United States of America"),
            new Country("GB", "zz", "Great Britain"),
        });
    }

    public ReadOnlyCollection<Country> Countries { get; }

    public async Task<IEnumerable<Country>> Search(string partialName)
    {
        if (partialName is null or "")
        {
            return Countries;
        }
        return Countries.Where(x => x.Name.Contains(partialName));
    }
}

public interface ICountriesContext : ISingletonService
{
    ReadOnlyCollection<Country> Countries { get; }
    Task<IEnumerable<Country>> Search(string partialName);
}
