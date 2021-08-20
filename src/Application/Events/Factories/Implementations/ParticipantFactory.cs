using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Event.Participants;

namespace EnduranceJudge.Application.Events.Factories.Implementations
{
    public class ParticipantFactory : IParticipantFactory
    {
        public Participant Create(ParticipantDependantModel data)
        {
            var participant = new Participant(data);
            return participant;
        }
    }
}
