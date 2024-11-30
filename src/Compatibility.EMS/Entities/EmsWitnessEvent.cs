using NTS.Compatibility.EMS.Entities.LapRecords;

namespace NTS.Compatibility.EMS.Entities;

public class EmsWitnessEvent
{
    public WitnessEventType Type { get; set; }
    public string TagId { get; set; }
    public DateTime Time { get; set; }
    public bool IsFromWitnessApp { get; set; }
}
