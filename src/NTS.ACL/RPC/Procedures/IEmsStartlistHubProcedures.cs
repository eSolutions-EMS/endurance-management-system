using NTS.ACL.Entities;

namespace NTS.ACL.RPC.Procedures;

public interface IEmsStartlistHubProcedures
{
    Dictionary<int, EmsStartlist> SendStartlist();
}
