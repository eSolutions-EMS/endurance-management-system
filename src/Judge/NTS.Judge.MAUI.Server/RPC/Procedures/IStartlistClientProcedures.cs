using NTS.Domain.Core.Objects.Startlists;

namespace NTS.Judge.MAUI.Server.RPC.Procedures;

public interface IStartlistClientProcedures
{
    Task ReceiveEntry(StartlistEntry entry);
}
