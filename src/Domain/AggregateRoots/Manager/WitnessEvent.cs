using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager
{
    public class WitnessEvent
    {
        public WitnessEventType Type { get; init; }
        public string TagId { get; init; }
        public DateTime Time { get; init; }
    }

    public enum WitnessEventType
    {
        Invalid = 0,
        EnterVet = 1,
        Finish = 2,
    }
}
