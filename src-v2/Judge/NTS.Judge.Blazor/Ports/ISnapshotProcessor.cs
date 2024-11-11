using Not.Injection;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface ISnapshotProcessor : ISingleton
{
    Task Process(Snapshot snapshot);
}
