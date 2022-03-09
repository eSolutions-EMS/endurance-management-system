using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.TimeRecords;
using EnduranceJudge.Gateways.Persistence.Core;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class TimeRecordQueries : QueriesBase<TimeRecord>
    {
        public TimeRecordQueries(IState state) : base(state)
        {
        }

        protected override List<TimeRecord> Set => this.State
            .Participants
            .SelectMany(part => part.TimeRecords)
            .ToList();
    }
}
