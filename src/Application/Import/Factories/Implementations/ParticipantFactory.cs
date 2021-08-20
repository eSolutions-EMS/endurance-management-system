using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Domain.Aggregates.Import.Participants;

namespace EnduranceJudge.Application.Import.Factories.Implementations
{
    public class ParticipantFactory : IParticipantFactory
    {
        public Participant Create(Athlete athlete, Horse horse)
        {
            var participant = new Participant(athlete, horse);
            return participant;
        }
    }
}
