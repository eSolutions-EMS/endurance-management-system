namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsEmsParticipantstHubProcedures
{
    EmsParticipantsPayload SendParticipants();
    Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type);
}
