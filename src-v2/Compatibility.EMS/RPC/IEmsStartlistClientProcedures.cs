using NTS.Compatibility.EMS.Enums;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsStartlistClientProcedures
{
    Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action);
}
