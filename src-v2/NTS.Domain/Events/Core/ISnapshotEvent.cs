using Not.Events;
namespace NTS.Domain;

public interface ISnapshotEvent<T> : IEvent
    where T : ISnapshot
{
    int Number { get; }
    T Snapshot { get; }
}