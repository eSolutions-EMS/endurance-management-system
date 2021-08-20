using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Event.Phases
{
    public class PhaseException : DomainException
    {
        protected override string Entity { get; } = nameof(Phase);
    }
}
