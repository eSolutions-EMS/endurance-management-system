using EMS.Core.Application.Core;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.LapRecords;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Application.Queries;

public class TimeRecordQueries : QueriesBase<LapRecord>
{
    public TimeRecordQueries(IStateContext context) : base(context)
    {
    }

    protected override List<LapRecord> Set => this.State
        .Participants
        .SelectMany(part => part.LapRecords)
        .ToList();
}
