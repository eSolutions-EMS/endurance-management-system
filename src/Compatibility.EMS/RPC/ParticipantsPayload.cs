using NTS.Compatibility.EMS.Entities;

namespace NTS.Judge.Tests.RPC;

public class ParticipantsPayload
{
    public List<EmsParticipantEntry> Participants { get; set; } = new();
    public int EventId { get; set; }
}
