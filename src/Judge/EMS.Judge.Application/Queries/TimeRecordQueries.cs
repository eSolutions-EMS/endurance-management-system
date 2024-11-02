using System.Collections.Generic;
using System.Linq;
using Core.Domain.State;
using Core.Domain.State.LapRecords;
using EMS.Judge.Application.Common;

namespace EMS.Judge.Application.Queries;

public class TimeRecordQueries : QueriesBase<LapRecord>
{
    public TimeRecordQueries(IStateContext context)
        : base(context) { }

    protected override List<LapRecord> Set =>
        this.State.Participants.SelectMany(part => part.LapRecords).ToList();
}
