namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsStartlistHubProcedures
{
    Dictionary<int, EmsStartlist> SendStartlist();
}