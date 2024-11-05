using System;
using Core.Domain.State.LapRecords;

namespace Core.Domain.AggregateRoots.Common.Performances;

public interface IPerformance : ILapRecordState
{
    DateTime? RequiredInspectionTime { get; }
    TimeSpan? RecoverySpan { get; }
    TimeSpan? Time { get; }
    double? AverageSpeed { get; }
    double? AverageSpeedPhase { get; }
    public double TotalLength { get; }
    public DateTime? NextStartTime { get; }
}
