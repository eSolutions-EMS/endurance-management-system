
using System.Diagnostics.CodeAnalysis;

namespace NTS.Domain.Core.Objects;

public record Snapshot : ISnapshot
{
    private SnapshotType _snapshotType;
    private SnapshotMethod _snapshotMethod;
    private Timestamp _timestamp = default!;

    public SnapshotType Type
    {
        get => _snapshotType;
        private set
        {
            GuardHelper.ThrowIfDefault(value);
            _snapshotType = value;
        }
    }

    public SnapshotMethod Method
    {
        get => _snapshotMethod;
        private set
        {
            GuardHelper.ThrowIfDefault(value);
            _snapshotMethod = value;
        }
    }

    [NotNull]
    public Timestamp Timestamp
    {
        get => _timestamp;
        private set
        {
            GuardHelper.ThrowIfDefault(value);
            _timestamp = value;
        }
    }
}
