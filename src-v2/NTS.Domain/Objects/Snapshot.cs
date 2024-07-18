using NTS.Domain.Enums;

namespace NTS.Domain.Objects;

public record Snapshot : DomainObject, ISnapshot
{
    public Snapshot(int number, SnapshotType type, SnapshotMethod method)
    {
        Number = number;
        Type = type;
        Method = method;
        Timestamp = Timestamp.Now();
    }

    public int Number { get; }
    public SnapshotType Type { get; }
    public SnapshotMethod Method { get; }
    public Timestamp Timestamp { get; }
}