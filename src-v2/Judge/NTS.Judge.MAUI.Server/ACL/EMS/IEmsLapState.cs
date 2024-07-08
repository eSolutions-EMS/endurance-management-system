using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsLapState : IIdentifiable
{
    double LengthInKm { get; }
    bool IsFinal { get; }
    int OrderBy { get; }
    int MaxRecoveryTimeInMins { get; }
    int RestTimeInMins { get; }
    bool IsCompulsoryInspectionRequired { get; }
}
