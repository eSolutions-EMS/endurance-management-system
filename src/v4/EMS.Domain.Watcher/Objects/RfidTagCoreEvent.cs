using Not.Domain;
using EMS.Domain.Watcher.Entities;

namespace EMS.Domain.Watcher.Objects;

public record RfidTagCoreEvent : DomainObject, ICoreEvent
{
    public RfidTagCoreEvent(RfidTag tag, CoreEventType type, Timestamp timestamp)
    {
        CoreId = new RfidTagCoreIdentifier(tag);
        Type = type;
        Timestamp = timestamp;
    }
    public CoreIdentifier CoreId { get; }
    public CoreEventType Type { get; }
    public Timestamp Timestamp { get; }
}
