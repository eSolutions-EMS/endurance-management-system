using NTS.Compatibility.EMS.Enums;
using NTS.Compatibility.EMS.Entities.EMS;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsParticipantsClientProcedures
{
    Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action);
}
