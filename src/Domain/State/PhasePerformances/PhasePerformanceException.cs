using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.PhasePerformances
{
    public class PhasePerformanceException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(PhasePerformance);
    }
}
