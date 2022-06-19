using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class CompetitionQueries : QueriesBase<Competition>
{
    public CompetitionQueries(IStateContext context) : base(context)
    {
    }
    protected override List<Competition> Set => this.State.Event.Competitions.ToList();
}
