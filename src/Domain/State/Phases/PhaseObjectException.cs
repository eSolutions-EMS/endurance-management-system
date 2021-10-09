using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Phases
{
    public class PhaseObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Phase);
    }
}
