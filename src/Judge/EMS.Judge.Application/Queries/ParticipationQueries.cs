using System.Collections.Generic;
using System.Linq;
using Core.Domain.State;
using Core.Domain.State.Participations;
using EMS.Judge.Application.Common;

namespace EMS.Judge.Application.Queries;

public class ParticipationQueries : QueriesBase<Participation>
{
    public ParticipationQueries(IStateContext context)
        : base(context) { }

    protected override List<Participation> Set => this.State.Participations.ToList();
}
