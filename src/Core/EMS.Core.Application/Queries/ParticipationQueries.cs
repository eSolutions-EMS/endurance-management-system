using EMS.Core.Application.Core;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Participations;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Application.Queries;

public class ParticipationQueries : QueriesBase<Participation>
{
    public ParticipationQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Participation> Set => this.State.Participations.ToList();
}
