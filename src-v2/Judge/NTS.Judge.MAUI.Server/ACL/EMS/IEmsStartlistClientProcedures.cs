namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsStartlistClientProcedures
{
    Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action);
}
