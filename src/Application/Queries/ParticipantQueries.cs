using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Participants;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class ParticipantQueries : QueriesBase<Participant>
{
    public ParticipantQueries(IStateContext context) : base(context)
    {
    }

    protected override List<Participant> Set => this.State.Participants.ToList();
}
