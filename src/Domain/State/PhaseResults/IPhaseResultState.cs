using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.PhaseResults
{
    public interface IPhaseResultState : IIdentifiable
    {
        bool IsRanked { get; }

        string Code { get; }
    }
}
