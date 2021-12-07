using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Performances
{
    public class PerformanceException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Performance);
    }
}
