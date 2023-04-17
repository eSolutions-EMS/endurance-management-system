using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager
{
    public abstract class WitnessEventBase
    {
        public WitnessEventType Type { get; set; }
        public DateTime Time { get; set; }
    }

    public class WitnessEvent : WitnessEventBase
    {
        public string TagId { get; init; }
    }

    public class ManualWitnessEvent : WitnessEventBase
    {
        public int Number { get; set; }
    }

    public enum WitnessEventType
    {
        VetIn = 1,
        Arrival = 2,
    }
}
