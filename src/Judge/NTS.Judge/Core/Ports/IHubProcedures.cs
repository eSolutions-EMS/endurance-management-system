using NTS.Domain.Objects;

namespace NTS.Judge.Core.Ports;

public interface IHubProcedures
{
    Task ReceiveSnapshot(Snapshot snapshot);
}
