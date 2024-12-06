using NTS.ACL.Entities.LapRecords;

namespace NTS.ACL.Entities;

public class EmsWitnessEvent
{
    public WitnessEventType Type { get; set; }
    public string TagId { get; set; }
    public DateTime Time { get; set; }
    public bool IsFromWitnessApp { get; set; }
}
