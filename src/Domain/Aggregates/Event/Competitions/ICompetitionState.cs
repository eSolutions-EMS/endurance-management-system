using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.Aggregates.Event.Competitions
{
    public interface ICompetitionState : IDomainModelState
    {
        CompetitionType Type { get; }
        string Name { get; }
    }
}
