using Not.Injection;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Contexts;

public class CountriesContext : ICountriesContext
{
    public Task<IEnumerable<Country>> Search(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            var countries = StaticOptions.Countries.AsEnumerable();
            return Task.FromResult(countries);
        }
        var result = StaticOptions.Countries.Where(x =>
            x.Name.Contains(term, StringComparison.InvariantCultureIgnoreCase)
        );
        return Task.FromResult(result);
    }
}

public interface ICountriesContext : ISingleton
{
    Task<IEnumerable<Country>> Search(string partialName);
}
