using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Laps;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class LapQueries : QueriesBase<Lap>
{
    public LapQueries(IState state) : base(state)
    {
    }
    protected override List<Lap> Set
        => this.State
            .Event
            .Competitions
            .SelectMany(x => x.Laps)
            .ToList();
}
