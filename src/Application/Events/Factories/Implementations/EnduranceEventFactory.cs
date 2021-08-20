using EnduranceJudge.Application.Events.Commands.EnduranceEvents;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using System.Linq;

namespace EnduranceJudge.Application.Events.Factories.Implementations
{
    public class EnduranceEventFactory : IEnduranceEventFactory
    {
        private readonly ICompetitionFactory competitionFactory;
        private readonly IPersonnelFactory personnelFactory;

        public EnduranceEventFactory(ICompetitionFactory competitionFactory, IPersonnelFactory personnelFactory)
        {
            this.competitionFactory = competitionFactory;
            this.personnelFactory = personnelFactory;
        }

        public EnduranceEvent Create(SaveEnduranceEvent data)
        {
            var enduranceEvent = new EnduranceEvent(data);

            foreach (var competition in data.Competitions.Select(this.competitionFactory.Create))
            {
                enduranceEvent.Add(competition);
            }

            foreach (var personnel in data.Personnel.Select(this.personnelFactory.Create))
            {
                enduranceEvent.Add(personnel);
            }

            return enduranceEvent;
        }
    }
}
