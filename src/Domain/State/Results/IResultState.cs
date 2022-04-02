using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Results;

public interface IResultState : IIdentifiable
{
    bool IsDisqualified { get; }

    string Code { get; }
}
