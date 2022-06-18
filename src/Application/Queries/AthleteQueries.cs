using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class AthleteQueries : QueriesBase<Athlete>
{
    public AthleteQueries(IState state) : base(state)
    {
    }

    protected override List<Athlete> Set => this.State.Athletes.ToList();
}
