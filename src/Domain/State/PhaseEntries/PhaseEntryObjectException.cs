using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.PhaseEntries
{
    public class PhaseEntryObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(PhaseEntry);
    }
}
