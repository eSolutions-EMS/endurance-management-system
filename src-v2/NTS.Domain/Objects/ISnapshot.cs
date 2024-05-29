using NTS.Domain.Enums;

namespace NTS.Domain.Objects;

public interface ISnapshot
{
    public SnapshotType Type { get; }
    public SnapshotMethod Method { get; }
    public Timestamp Timestamp { get; }
}
