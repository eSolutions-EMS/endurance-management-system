using EnduranceJudge.Domain.Aggregates.Import.Competitions;
using EnduranceJudge.Domain.Aggregates.Import.Participants;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Import.Factories.Implementations
{
    public class CompetitionFactory : ICompetitionFactory
    {
        public Competition  Create(string name, List<Participant> participants)
        {
            var competition = new Competition(name, participants);
            return competition;
        }
    }
}
