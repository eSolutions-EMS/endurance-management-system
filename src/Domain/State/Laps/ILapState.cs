using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Laps;

public interface ILapState : IIdentifiable
{
    double LengthInKm { get; }
    bool IsFinal { get; }
    int OrderBy { get; }
    int MaxRecoveryTimeInMins { get; }
    int RestTimeInMins { get; }
    bool IsCompulsoryInspectionRequired { get; }
}