using NTS.Compatibility.EMS.Entities.Laps;

namespace NTS.Judge.ACL.Bridge;

public class EmsLapState : IEmsLapState
{
    public double LengthInKm { get; set; }
    public bool IsFinal { get; set; }
    public int OrderBy { get; set; }
    public int MaxRecoveryTimeInMins { get; set; }
    public int RestTimeInMins { get; set; }
    public bool IsCompulsoryInspectionRequired { get; set; }
    public int Id { get; set; }
}
