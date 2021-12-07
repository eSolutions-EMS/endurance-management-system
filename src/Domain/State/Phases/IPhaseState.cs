using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Phases
{
    public interface IPhaseState : IDomainModelState
    {
        double LengthInKm { get; }
        bool IsFinal { get; }
        int OrderBy { get; }
        int MaxRecoveryTimeInMins { get; }
        int RestTimeInMins { get; }
        bool RequireCompulsoryInspection { get; }
    }
}
