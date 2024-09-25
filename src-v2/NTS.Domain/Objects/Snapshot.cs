using NTS.Domain.Enums;

namespace NTS.Domain.Objects;

public abstract record Snapshot : DomainObject, ISnapshot
{
    protected Snapshot(int number, SnapshotType type, SnapshotMethod method, Timestamp timestamp)
    {
        Number = number;
        Type = type;
        Method = method;
        Timestamp = timestamp;
    }

    public int Number { get; }
    public SnapshotType Type { get; }
    public SnapshotMethod Method { get; }
    public Timestamp Timestamp { get; }
}

public record StageSnapshot : Snapshot
{
    public StageSnapshot(int number, SnapshotMethod method, Timestamp timestamp) : base(number, SnapshotType.Stage, method, timestamp)
    {
    }
}

public record FinishSnapshot : Snapshot
{
    public FinishSnapshot(int number, SnapshotMethod method, Timestamp timestamp) : base(number, SnapshotType.Final, method, timestamp)
    {
    }
}

public record VetgateSnapshot : Snapshot
{
    public VetgateSnapshot(int number, SnapshotMethod method, Timestamp timestamp) : base(number, SnapshotType.Vet, method, timestamp)
    {
    }
}