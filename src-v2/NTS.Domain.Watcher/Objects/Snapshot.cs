using Not.Domain;
using NTS.Domain.Enums;
using NTS.Domain.Objects;

namespace NTS.Domain.Watcher.Objects;

public record Snapshot : DomainObject, ISnapshot
{
    public Snapshot(SnapshotType type, SnapshotMethod method)
    {
        Type = type;
        Method = method;
        Timestamp = new Timestamp();
    }

    public SnapshotType Type { get; }
    public SnapshotMethod Method { get; }
    public Timestamp Timestamp { get; }
}
