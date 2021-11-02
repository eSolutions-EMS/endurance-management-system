using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.PhaseResults
{
    public class PhaseResultException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(PhaseResult);
    }
}
