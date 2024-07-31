using NTS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NTS.Domain.Objects;

public record Snapshot : DomainObject, ISnapshot
{
    public Snapshot(int number, SnapshotType type, SnapshotMethod method, Timestamp timestamp)
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