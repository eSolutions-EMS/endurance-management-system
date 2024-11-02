using Not.Domain;
using NTS.Domain.Enums;
using NTS.Domain.Objects;

namespace NTS.Domain.Watcher.Objects;

public record RFIDSnpashot : Snapshot
{
    public RFIDSnpashot(int number, SnapshotType type)
        : base(number, type, SnapshotMethod.RFID, Timestamp.Now()) { }
}

public record ManualSnapshot : Snapshot
{
    public ManualSnapshot(int number, SnapshotType type)
        : base(number, type, SnapshotMethod.Manual, Timestamp.Now()) { }
}
