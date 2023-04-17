using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;

namespace Endurance.Judge.Gateways.API.Requests
{
    public class WitnessRequest
    {
        public int Number { get; set; }
        public DateTimeOffset Time { get; set; }
        public WitnessEventType Type { get; set; }
    }
}
