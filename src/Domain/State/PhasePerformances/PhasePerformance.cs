using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.PhaseResults;
using EnduranceJudge.Domain.State.Phases;
using System;

namespace EnduranceJudge.Domain.State.PhasePerformances
{
    public class PhasePerformance : DomainObjectBase<PhasePerformanceException>, IPhasePerformanceState
    {
        private PhasePerformance() {}
        public PhasePerformance(Phase phase, DateTime startTime) : base(GENERATE_ID)
        {
            this.Phase = phase;
            this.StartTime = startTime;
        }

        public Phase Phase { get; }
        public DateTime StartTime { get; }
        public DateTime? ArrivalTime { get; internal set; }
        public DateTime? InspectionTime { get; internal set; }
        public DateTime? ReInspectionTime { get; internal set; }
        public PhaseResult Result { get; internal set; }
    }
}
