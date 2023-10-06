using Common.Domain;
using EMS.Domain.Watcher.Entities;

namespace EMS.Domain.Watcher.Objects;

public record RfidTagPhaseEvent : DomainObject, IPhaseEvent
{
    public RfidTagPhaseEvent(RfidTag tag, PhaseEventType type, Timestamp timestamp)
    {
        CoreId = new RfidTagCoreIdentifier(tag);
        Type = type;
        Timestamp = timestamp;
    }
    public CoreIdentifier CoreId { get; }
    public PhaseEventType Type { get; }
    public Timestamp Timestamp { get; }
}
