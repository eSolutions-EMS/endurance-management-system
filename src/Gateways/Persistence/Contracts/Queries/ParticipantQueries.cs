using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries;

public class ParticipantQueries : QueriesBase<Participant>
{
    public ParticipantQueries(IState state) : base(state)
    {
    }

    protected override List<Participant> Set => this.State.Participants.ToList();
}