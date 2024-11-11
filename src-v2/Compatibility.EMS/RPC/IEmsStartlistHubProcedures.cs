using NTS.Compatibility.EMS.Entities;

namespace NTS.Compatibility.EMS.RPC;

public interface IEmsStartlistHubProcedures
{
    Dictionary<int, EmsStartlist> SendStartlist();
}
