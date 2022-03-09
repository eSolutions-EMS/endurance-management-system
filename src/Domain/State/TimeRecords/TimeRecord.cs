using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Phases;
using EnduranceJudge.Domain.State.Results;
using System;

namespace EnduranceJudge.Domain.State.TimeRecords;

public class TimeRecord : DomainBase<TimeRecordException>, ITimeRecordState
{
    public TimeRecord() {}
    public TimeRecord(DateTime startTime, Phase phase) : base(GENERATE_ID)
    {
        this.StartTime = startTime;
        this.Phase = phase;
    }

    public Phase Phase { get; private set; }
    public DateTime StartTime { get; set; } // TODO: set to private/internal after testing
    public DateTime? ArrivalTime { get; set; } // TODO: set to private/internal after testing
    public DateTime? InspectionTime { get; set; } // TODO: set to private/internal after testing
    public DateTime? ReInspectionTime { get; set; } // TODO: set to private/internal after testing
    public bool IsReInspectionRequired { get; internal set; }
    public bool IsRequiredInspectionRequired { get; internal set; }
    public Result Result { get; internal set; }

    public DateTime? VetGateTime
        => this.ReInspectionTime ?? this.InspectionTime;
}
