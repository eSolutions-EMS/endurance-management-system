using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries;

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