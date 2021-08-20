using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInPhases
{
    public interface IParticipationInPhaseState : IDomainModelState
    {
        DateTime StartTime { get; }

        DateTime? ArrivalTime { get; }

        DateTime? InspectionTime { get; }

        DateTime? ReInspectionTime { get; }
    }
}
