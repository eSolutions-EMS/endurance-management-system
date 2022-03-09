using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Results
{
    public class PhaseResultException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(Result);
    }
}
