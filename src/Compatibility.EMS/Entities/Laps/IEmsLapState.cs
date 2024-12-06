using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Laps;

public interface IEmsLapState : IEmsIdentifiable
{
    double LengthInKm { get; }
    bool IsFinal { get; }
    int OrderBy { get; }
    int MaxRecoveryTimeInMins { get; }
    int RestTimeInMins { get; }
    bool IsCompulsoryInspectionRequired { get; }
}
