using NTS.Domain.Objects;

namespace NTS.Application.RPC.Procedures;

public interface IHubProcedures
{
    Task ReceiveSnapshot(Snapshot snapshot);
}
