using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.State.PhaseEntries
{
    public class PhaseEntryException : DomainException
    {
        protected override string Entity { get; } = nameof(PhaseEntry);
    }
}
