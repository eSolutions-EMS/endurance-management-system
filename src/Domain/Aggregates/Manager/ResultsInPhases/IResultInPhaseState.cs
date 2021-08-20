using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Manager.ResultsInPhases
{
    public interface IResultInPhaseState : IDomainModelState
    {
        bool IsRanked { get; }

        string Code { get; }
    }
}
