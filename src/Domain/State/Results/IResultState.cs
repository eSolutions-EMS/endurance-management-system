using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Results
{
    public interface IResultState : IIdentifiable
    {
        bool IsRanked { get; }

        string Code { get; }
    }
}
