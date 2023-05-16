using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Athletes;
using EMS.Judge.Application.Core;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Queries;

public class AthleteQueries : QueriesBase<Athlete>
{
    public AthleteQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Athlete> Set => this.State.Athletes.ToList();
}
