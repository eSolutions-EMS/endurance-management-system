using NTS.Compatibility.EMS.Entities;
using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsEmsParticipantstHubProcedures
{
    EmsParticipantsPayload SendParticipants();
    Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type);
}
