using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Participants;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class ParticipantQueries : QueriesBase<Participant>
{
    public ParticipantQueries(IState state) : base(state)
    {
    }

    protected override List<Participant> Set => this.State.Participants.ToList();
}
