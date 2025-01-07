using NTS.ACL.Entities;
using NTS.ACL.Enums;

namespace NTS.ACL.RPC.Procedures;

public interface IEmsParticipantsClientProcedures
{
    Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action);
}
