using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Contracts
{
    public class State : IState
    {
        public EnduranceEvent Event { get; set; }
        public List<Horse> Horses { get; } = new List<Horse>();
        public List<Athlete> Athletes { get; } = new List<Athlete>();
        public List<Participant> Participants { get; } = new List<Participant>();
    }
}
