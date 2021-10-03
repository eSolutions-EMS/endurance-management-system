using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Manager.ResultsInPhases
{
    public class ManagerResultInPhaseException : DomainException
    {
        protected override string Entity { get; } = $"Manager {nameof(ResultInPhase)}";
    }
}
