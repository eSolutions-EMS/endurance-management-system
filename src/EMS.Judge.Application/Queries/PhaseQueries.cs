using EMS.Core.Application.Core;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Laps;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Application.Queries;

public class LapQueries : QueriesBase<Lap>
{
    public LapQueries(IStateContext context) : base(context)
    {
    }
    protected override List<Lap> Set
        => this.State
            .Event
            .Competitions
            .SelectMany(x => x.Laps)
            .ToList();
}
