using NTS.Domain.Objects;

namespace NTS.Application.RPC;

public interface IHubProcedures
{
    Task ReceiveSnapshot(Snapshot snapshot);
}
