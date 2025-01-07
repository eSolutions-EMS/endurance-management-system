using NTS.ACL.Entities;

namespace NTS.ACL.RPC;

public class ParticipantsPayload
{
    public List<EmsParticipantEntry> Participants { get; set; } = new();
    public int EventId { get; set; }
}
