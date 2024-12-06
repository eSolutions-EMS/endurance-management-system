using NTS.ACL.Entities;
using NTS.ACL.Entities.EMS;

namespace NTS.ACL.RPC.Procedures;

public interface IEmsEmsParticipantstHubProcedures
{
    EmsParticipantsPayload SendParticipants();
    Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type);
}
