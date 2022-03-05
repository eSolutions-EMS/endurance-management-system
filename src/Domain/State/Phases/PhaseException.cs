using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Phases
{
    public class PhaseException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(Phase);
    }
}
