using NTS.Compatibility.EMS.Entities;
using NTS.Compatibility.EMS.Enums;

namespace NTS.Compatibility.EMS.RPC.Procedures;

public interface IEmsParticipantsClientProcedures
{
    Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action);
}
