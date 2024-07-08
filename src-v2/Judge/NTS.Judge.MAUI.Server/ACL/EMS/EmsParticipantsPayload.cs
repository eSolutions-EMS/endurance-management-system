namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsParticipantsPayload
{
    public List<EmsParticipantEntry> Participants { get; set; } = new();
    public int EventId { get; set; }
}
