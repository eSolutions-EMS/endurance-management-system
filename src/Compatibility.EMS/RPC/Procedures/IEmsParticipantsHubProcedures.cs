using NTS.Compatibility.EMS.Entities;
using NTS.Compatibility.EMS.Entities.EMS;

namespace NTS.Compatibility.EMS.RPC.Procedures;

public interface IEmsEmsParticipantstHubProcedures
{
    EmsParticipantsPayload SendParticipants();
    Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type);
}
