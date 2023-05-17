using System;

namespace Core.Domain.AggregateRoots.Manager;

public class WitnessEvent
{
    public WitnessEventType Type { get; set; } 
    public string TagId { get; set; }
    public DateTime Time { get; set; }
}

public enum WitnessEventType
{
    VetIn = 1,
    Arrival = 2,
}
