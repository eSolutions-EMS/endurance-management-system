using System;

namespace EnduranceJudge.Domain.State.TimeRecords;

public interface ITimeRecordState
{
    DateTime StartTime { get; }
    DateTime? ArrivalTime { get; }
    DateTime? InspectionTime { get; }
    DateTime? ReInspectionTime { get; }
    bool IsRequiredInspectionRequired { get; }
}
