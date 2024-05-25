namespace NTS.Domain.Core.Aggregates.Participations;

public class SnapshotResult : DomainEntity
{
    internal SnapshotResult(Snapshot snapshot) : this(snapshot, SnapshotResultType.Applied)
    {
    }
    internal SnapshotResult(Snapshot snapshot, SnapshotResultType type)
    {
        Snapshot = snapshot;
        Type = type;
    }

    public Snapshot Snapshot { get; }
    public SnapshotResultType Type { get; }
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
