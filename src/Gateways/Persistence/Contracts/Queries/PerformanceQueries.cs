using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class PerformanceQueries : QueriesBase<Performance>
    {
        public PerformanceQueries(IState state) : base(state)
        {
        }

        protected override List<Performance> Set => this.State
            .Participants
            .Select(x => x.Participation)
            .SelectMany(part => part.Performances)
            .ToList();
    }
}
