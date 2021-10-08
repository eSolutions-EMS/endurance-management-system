using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class CountryQueries : QueriesBase<Country>, IRepository
    {
        public CountryQueries(IDataContext context) : base(context)
        {
        }

        protected override List<Country> Set => this.Context.State.Countries;
    }
}
