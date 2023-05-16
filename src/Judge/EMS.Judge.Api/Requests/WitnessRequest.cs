using EMS.Core.Domain.AggregateRoots.Manager;
using System;

namespace EMS.Judge.Api.Requests
{
    public class WitnessRequest
    {
        public int Number { get; set; }
        public DateTimeOffset Time { get; set; }
        public WitnessEventType Type { get; set; }
    }
}
