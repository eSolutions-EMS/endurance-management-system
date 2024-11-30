using NTS.Compatibility.EMS.Entities;

namespace NTS.Compatibility.EMS.RPC.Procedures;

public interface IEmsStartlistHubProcedures
{
    Dictionary<int, EmsStartlist> SendStartlist();
}
