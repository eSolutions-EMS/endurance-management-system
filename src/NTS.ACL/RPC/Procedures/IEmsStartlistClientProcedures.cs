using NTS.ACL.Enums;

namespace NTS.ACL.RPC.Procedures;

public interface IEmsStartlistClientProcedures
{
    Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action);
}
