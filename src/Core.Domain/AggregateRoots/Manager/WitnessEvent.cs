using Core.Domain.State.Participants;
using System;

namespace Core.Domain.AggregateRoots.Manager;

public class WitnessEvent
{
    public WitnessEventType Type { get; set; } 
    public string TagId { get; set; }
    public DateTime Time { get; set; }
    public bool IsFromWitnessApp { get; set; }
}

public class RfidTagEvent : WitnessEvent
{
    public RfidTagEvent(RfidTag tag)
    {
        this.Tag = tag;
        this.TagId = tag.ParticipantNumber;
    }

    public RfidTag Tag { get; private set; }
}

public enum WitnessEventType
{
    VetIn = 1,
    Arrival = 2,
}
