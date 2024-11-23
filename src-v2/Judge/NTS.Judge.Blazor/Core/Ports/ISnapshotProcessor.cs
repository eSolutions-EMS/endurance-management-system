using Not.Injection;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Core.Ports;

public interface ISnapshotProcessor : ISingleton
{
    Task Process(Snapshot snapshot);
}
