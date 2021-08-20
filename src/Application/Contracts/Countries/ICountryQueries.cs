using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Common.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Contracts.Countries
{
    public interface ICountryQueries : IService
    {
        Task<Country> Find(string isoCode);

        Task<IList<T>> All<T>();
    }
}
