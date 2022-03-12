using EnduranceJudge.Core.Models;
using System;

namespace EnduranceJudge.Domain.State.TimeRecords;

public interface ILapRecordState : IIdentifiable
{
    DateTime StartTime { get; }
    DateTime? ArrivalTime { get; }
    DateTime? InspectionTime { get; }
    DateTime? ReInspectionTime { get; }
    bool IsReInspectionRequired { get; }
    bool IsRequiredInspectionRequired { get; }
}
