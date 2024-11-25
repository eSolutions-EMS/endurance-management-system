using NTS.Compatibility.EMS.Entities.EMS;

namespace NTS.Compatibility.EMS.Entities;

public class EmsParticipantsPayload
{
    public List<EmsParticipantEntry> Participants { get; set; } = [];
    public int EventId { get; set; }
}
