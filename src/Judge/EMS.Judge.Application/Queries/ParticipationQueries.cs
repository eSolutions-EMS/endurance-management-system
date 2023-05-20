using Core.Domain.State;
using Core.Domain.State.Participations;
using EMS.Judge.Application.Common;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Queries;

public class ParticipationQueries : QueriesBase<Participation>
{
    public ParticipationQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Participation> Set => this.State.Participations.ToList();
}
