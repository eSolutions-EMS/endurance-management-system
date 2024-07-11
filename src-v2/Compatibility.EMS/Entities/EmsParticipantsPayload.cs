using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Compatibility.EMS.Entities;

public class EmsParticipantsPayload
{
    public List<EmsParticipantEntry> Participants { get; set; } = new();
    public int EventId { get; set; }
}
