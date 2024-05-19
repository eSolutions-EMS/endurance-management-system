using Not.Events;
namespace NTS.Domain;

public interface ICoreEvent : IEvent
{
    int Number { get; }
    ISnapshot Snapshot { get; }
}