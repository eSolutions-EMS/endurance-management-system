namespace NTS.Domain.Core.Aggregates.Participations;

public class SnapshotResult : DomainEntity
{
#pragma warning disable CS8618 // Deserialization ctor
    private SnapshotResult() { }
#pragma warning restore CS8618 

    public static SnapshotResult Applied(Snapshot snapshot) => new(snapshot, SnapshotResultType.Applied);
    public static SnapshotResult NotApplied(Snapshot snapshot, SnapshotResultType type) => new(snapshot, type);

    private SnapshotResult(Snapshot snapshot, SnapshotResultType type)
    {
        Snapshot = snapshot;
        Type = type;
    }

    public Snapshot Snapshot { get; private set; }
    public SnapshotResultType Type { get; private set; }
}

public enum SnapshotResultType
{
    Applied = 1,
    NotAppliedDueToNotQualified = 2,
    NotAppliedDueToComplete = 3,
    NotAppliedDueToNotStarted = 4,
    NotAppliedDueToSeparateStageLine = 5,
    NotAppliedDueToSeparateFinishLine = 6,
    NotAppliedDueToDuplicateArrive = 7,
    NotAppliedDueToDuplicateInsped = 8
}
