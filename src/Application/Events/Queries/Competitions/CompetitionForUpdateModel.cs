using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using System;

namespace EnduranceJudge.Application.Events.Queries.Competitions
{
    public class CompetitionForUpdateModel : ICompetitionState
    {
        public int Id { get; private set; }
        public CompetitionType Type { get; private set; }
        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }
    }
}
