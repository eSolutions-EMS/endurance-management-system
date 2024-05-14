using Not.Events;

namespace NTS.Domain;

public interface ICoreEvent : IEvent
{
    CoreIdentifier CoreId { get; }
    CoreEventType Type { get; }
    Timestamp Timestamp { get; }
}