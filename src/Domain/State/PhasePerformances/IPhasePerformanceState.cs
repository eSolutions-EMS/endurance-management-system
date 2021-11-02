using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Domain.State.PhasePerformances
{
    public interface IPhasePerformanceState : IDomainModelState
    {
        DateTime StartTime { get; }
        DateTime? ArrivalTime { get; }
        DateTime? InspectionTime { get; }
        DateTime? ReInspectionTime { get; }
    }
}
