using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Domain.State.TimeRecords;

public class TimeRecord : DomainBase<TimeRecordException>, ITimeRecordState
{
    public TimeRecord() {}
    public TimeRecord(DateTime startTime, DateTime? arrivalTime, DateTime? inspectionTime, DateTime? reInspectionTime)
        : base(GENERATE_ID)
    {
        this.StartTime = startTime;
        this.ArrivalTime = arrivalTime;
        this.InspectionTime = inspectionTime;
        this.ReInspectionTime = reInspectionTime;
    }

    public DateTime StartTime { get; private set; }
    public DateTime? ArrivalTime { get; private set; }
    public DateTime? InspectionTime { get; private set; }
    public DateTime? ReInspectionTime { get; private set; }
    public bool IsRequiredInspectionRequired { get; internal set; }
}
