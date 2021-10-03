using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.State.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Contracts
{
    public interface ICountryQueries : IService
    {
        Task<Country> Find(string isoCode);

        Task<IList<T>> All<T>();
    }
}
