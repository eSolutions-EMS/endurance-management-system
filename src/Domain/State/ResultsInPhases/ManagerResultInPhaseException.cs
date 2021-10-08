using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.ResultsInPhases
{
    public class ManagerResultInPhaseException : DomainException
    {
        protected override string Entity { get; } = $"Manager {nameof(ResultInPhase)}";
    }
}
