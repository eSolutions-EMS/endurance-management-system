using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Domain.States
{
    public interface IParticipationInPhaseState : IDomainModelState
    {
        DateTime StartTime { get; }
        DateTime? ArrivalTime { get; }
        DateTime? InspectionTime { get; }
        DateTime? ReInspectionTime { get; }
    }
}
