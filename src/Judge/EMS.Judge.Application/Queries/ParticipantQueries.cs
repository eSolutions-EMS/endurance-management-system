using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Participants;
using EMS.Judge.Application.Core;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Queries;

public class ParticipantQueries : QueriesBase<Participant>
{
    public ParticipantQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Participant> Set => this.State.Participants.ToList();
}
