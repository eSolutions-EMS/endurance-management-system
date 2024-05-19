
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Events;

public class SnapshotReceived : ISnapshotEvent<Snapshot>
{
    public SnapshotReceived(int number, Snapshot snapshot)
    {
        Guid = Guid.NewGuid();
        Number = number;
        Snapshot = snapshot;
    }

    public Guid Guid { get; }
    public int Number { get; }
    public Snapshot Snapshot { get; }
}
