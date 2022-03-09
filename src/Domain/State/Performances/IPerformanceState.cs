using EnduranceJudge.Domain.State.TimeRecords;
using System;

namespace EnduranceJudge.Domain.State.Performances
{
    public interface IPerformance : ITimeRecordState
    {
        DateTime? RequiredInspectionTime { get; }
        TimeSpan? RecoverySpan { get; }
        TimeSpan? Time { get; }
        double? AverageSpeed { get; }
        double? AverageSpeedTotal { get; }
        public double TotalLength { get; }
    }
}
