using NTS.Compatibility.EMS.Enums;

namespace NTS.Compatibility.EMS.RPC.Procedures;

public interface IEmsStartlistClientProcedures
{
    Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action);
}
