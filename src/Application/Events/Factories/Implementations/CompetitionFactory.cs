using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using System.Linq;

namespace EnduranceJudge.Application.Events.Factories.Implementations
{
    public class CompetitionFactory : ICompetitionFactory
    {
        private readonly IPhaseFactory phaseFactory;
        private readonly IParticipantFactory participantFactory;

        public CompetitionFactory(IPhaseFactory phaseFactory, IParticipantFactory participantFactory)
        {
            this.phaseFactory = phaseFactory;
            this.participantFactory = participantFactory;
        }

        public Competition Create(CompetitionDependantModel data)
        {
            var competition = new Competition(data);

            foreach (var phase in data.Phases.Select(this.phaseFactory.Create))
            {
                competition.Add(phase);
            }

            foreach (var participant in data.Participants.Select(this.participantFactory.Create))
            {
                competition.Add(participant);
            }

            return competition;
        }
    }
}
