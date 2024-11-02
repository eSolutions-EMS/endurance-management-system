using NTS.Compatibility.EMS.Entities.EMS;
using NTS.Compatibility.EMS.Enums;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsParticipantsClientProcedures
{
    Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action);
}
