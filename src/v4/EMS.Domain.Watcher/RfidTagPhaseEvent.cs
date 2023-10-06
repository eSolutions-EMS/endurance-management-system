using Common.Domain;
using Core.Domain.Common.Models;

namespace EMS.Domain.Watcher;

public record RfidTagPhaseEvent : DomainObject, IPhaseEvent
{
    public RfidTagPhaseEvent(RfidTag tag, PhaseEventType type, Timestamp timestamp)
    {
        this.CoreId = new RfidTagCoreIdentifier(tag);
        this.Type = type;
        this.Timestamp = timestamp;
    }
    public ICoreIdentifier CoreId { get; }
    public PhaseEventType Type { get; }
    public Timestamp Timestamp { get; }
}
