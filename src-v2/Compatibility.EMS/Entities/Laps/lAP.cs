using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Laps;

public class Lap : DomainBase<LapException>
{
    private Lap() {}

    public bool IsFinal { get; internal set; }
    public int OrderBy { get; internal set; }
    public double LengthInKm { get; internal set; }
    public int MaxRecoveryTimeInMins { get; internal set; }
    public int RestTimeInMins { get; internal set; }
    public bool IsCompulsoryInspectionRequired { get; internal set; }
}
