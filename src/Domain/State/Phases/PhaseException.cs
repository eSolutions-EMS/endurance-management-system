using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Phases
{
    public class PhaseException : DomainException
    {
        protected override string Entity { get; } = nameof(Phase);
    }
}
