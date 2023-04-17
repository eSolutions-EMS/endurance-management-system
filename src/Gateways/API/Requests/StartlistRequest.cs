using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Collections.Generic;

namespace Endurance.Judge.Gateways.API.Requests
{
    public class StartlistRequest
    {
        public IEnumerable<StartModel> Startlist { get; init; }
    }
}
