using System.Collections.Generic;
using System.Linq;
using Core.Domain.State;
using Core.Domain.State.Athletes;
using EMS.Judge.Application.Common;

namespace EMS.Judge.Application.Queries;

public class AthleteQueries : QueriesBase<Athlete>
{
    public AthleteQueries(IStateContext context)
        : base(context) { }

    protected override List<Athlete> Set => this.State.Athletes.ToList();
}
