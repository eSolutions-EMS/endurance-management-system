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

        public Phase Phase { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime? ArrivalTime { get; internal set; }
        public DateTime? InspectionTime { get; internal set; }
        public DateTime? ReInspectionTime { get; internal set; }
        public PhaseResult Result { get; internal set; }

        public TimeSpan? LoopSpan
            => this.ArrivalTime - this.StartTime;
        public TimeSpan? PhaseSpan
            => (this.ReInspectionTime ?? this.InspectionTime) - this.StartTime;
        public double? AverageSpeedForLoopInKpH
        {
            get
            {
                if (this.LoopSpan == null)
                {
                    return null;
                }
                return this.GetAverageSpeed(this.LoopSpan.Value);
            }
        }
        public double? AverageSpeedForPhaseInKpH
        {
            get
            {
                if (this.PhaseSpan == null)
                {
                    return null;
                }
                return this.GetAverageSpeed(this.PhaseSpan.Value);
            }
        }

        private double GetAverageSpeed(TimeSpan timeSpan)
        {
            var phaseLengthInKm = this.Phase.LengthInKm;
            var totalHours = timeSpan.TotalHours;
            return  phaseLengthInKm / totalHours;
        }
    }
}
