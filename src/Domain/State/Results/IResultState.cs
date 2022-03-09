using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.PhaseResults
{
    public interface IResultState : IIdentifiable
    {
        bool IsRanked { get; }

        string Code { get; }
    }
}
