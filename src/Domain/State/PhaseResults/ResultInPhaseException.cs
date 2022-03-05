using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.PhaseResults
{
    public class PhaseResultException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(PhaseResult);
    }
}
