using EnduranceJudge.Core.Models;
using System;

namespace EnduranceJudge.Domain.State.Performances
{
    public interface IPerformanceState : IIdentifiable
    {
        DateTime StartTime { get; }
        DateTime? ArrivalTime { get; }
        DateTime? InspectionTime { get; }
        DateTime? ReInspectionTime { get; }
        DateTime? RequiredInspectionTime { get; }
        DateTime? CompulsoryRequiredInspectionTime { get; }
        DateTime? NextPerformanceStartTime { get; }
        bool IsRequiredInspectionRequired { get; }
        TimeSpan? RecoverySpan { get; }
        TimeSpan? Time { get; }
        double? AverageSpeedTotalKpH { get; }
        public double LengthSoFar { get; }
    }
}
