using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.PhaseResults
{
    public interface IPhaseResultState : IDomainModelState
    {
        bool IsRanked { get; }

        string Code { get; }
    }
}
