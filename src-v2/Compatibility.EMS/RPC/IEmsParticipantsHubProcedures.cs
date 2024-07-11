using NTS.Compatibility.EMS.Entities;
using NTS.Compatibility.EMS.Entities.EMS;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsEmsParticipantstHubProcedures
{
    EmsParticipantsPayload SendParticipants();
    Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type);
}
