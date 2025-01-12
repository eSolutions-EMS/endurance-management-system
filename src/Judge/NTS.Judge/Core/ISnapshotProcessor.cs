using Not.Injection;
using NTS.Domain.Core.Aggregates;
using NTS.Domain.Objects;

namespace NTS.Judge.Core;

public interface ISnapshotProcessor : ISingleton
{
    Task<Participation> Process(Snapshot snapshot);
}
