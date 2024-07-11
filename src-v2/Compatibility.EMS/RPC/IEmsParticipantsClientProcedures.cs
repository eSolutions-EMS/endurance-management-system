using NTS.Compatibility.EMS.Enums;
using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsParticipantsClientProcedures
{
    Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action);
}
