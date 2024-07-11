using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Laps;

public class EmsLap : EmsDomainBase<EmsLapException>
{
    [Newtonsoft.Json.JsonConstructor]
    private EmsLap() {}
    public EmsLap(IEmsLapState state) : base(GENERATE_ID)
    {
        this.IsFinal = state.IsFinal;
        this.OrderBy = state.OrderBy;
        this.LengthInKm = state.LengthInKm;
        this.MaxRecoveryTimeInMins = state.MaxRecoveryTimeInMins;
        this.RestTimeInMins = state.RestTimeInMins;
        this.IsCompulsoryInspectionRequired = state.IsCompulsoryInspectionRequired;
    }

    public bool IsFinal { get; internal set; }
    public int OrderBy { get; internal set; }
    public double LengthInKm { get; internal set; }
    public int MaxRecoveryTimeInMins { get; internal set; }
    public int RestTimeInMins { get; internal set; }
    public bool IsCompulsoryInspectionRequired { get; internal set; }
}
