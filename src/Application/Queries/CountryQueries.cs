using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Countries;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class CountryQueries : QueriesBase<Country>
{
    public CountryQueries(IState state) : base(state)
    {
    }

    protected override List<Country> Set => this.State.Countries.ToList();
}
