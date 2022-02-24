using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Phases
{
    public interface IPhaseState : IIdentifiable
    {
        double LengthInKm { get; }
        bool IsFinal { get; }
        int OrderBy { get; }
        int MaxRecoveryTimeInMins { get; }
        int RestTimeInMins { get; }
        bool IsCompulsoryInspectionRequired { get; }
    }
}
