using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Results;

public interface IResultState : IIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
