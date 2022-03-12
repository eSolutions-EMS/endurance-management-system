using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Results;

public class ResultException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Result);
}
