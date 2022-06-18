using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.LapRecords;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Queries;

public class TimeRecordQueries : QueriesBase<LapRecord>
{
    public TimeRecordQueries(IState state) : base(state)
    {
    }

    protected override List<LapRecord> Set => this.State
        .Participants
        .SelectMany(part => part.LapRecords)
        .ToList();
}
