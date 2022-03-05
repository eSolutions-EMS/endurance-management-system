using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Performances
{
    public class PerformanceException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(Performance);
    }
}
