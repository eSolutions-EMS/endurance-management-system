using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Application.Events.Queries.Competitions
{
    public class CompetitionForUpdateModel : ICompetitionState
    {
        public int Id { get; private set; }
        public CompetitionType Type { get; private set; }
        public string Name { get; private set; }
    }
}
