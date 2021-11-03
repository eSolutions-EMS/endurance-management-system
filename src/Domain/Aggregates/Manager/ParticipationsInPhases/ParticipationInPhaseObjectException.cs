using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances
{
    public class PhasePerformanceObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(PhasePerformanceManagerOld);
    }
}
