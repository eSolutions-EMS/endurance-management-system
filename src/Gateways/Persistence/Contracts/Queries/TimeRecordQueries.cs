using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.LapRecords;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class TimeRecordQueries : QueriesBase<LapRecord>
    {
        public TimeRecordQueries(IState state) : base(state)
        {
        }

        protected override List<LapRecord> Set => this.State
            .Participants
            .SelectMany(part => part.TimeRecords)
            .ToList();
    }
}
