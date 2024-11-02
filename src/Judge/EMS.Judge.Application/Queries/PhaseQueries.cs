using System.Collections.Generic;
using System.Linq;
using Core.Domain.State;
using Core.Domain.State.Laps;
using EMS.Judge.Application.Common;

namespace EMS.Judge.Application.Queries;

public class LapQueries : QueriesBase<Lap>
{
    public LapQueries(IStateContext context)
        : base(context) { }

    protected override List<Lap> Set =>
        this.State.Event.Competitions.SelectMany(x => x.Laps).ToList();
}
