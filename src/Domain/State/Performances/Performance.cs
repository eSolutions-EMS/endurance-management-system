using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.PhaseResults;
using EnduranceJudge.Domain.State.Phases;
using System;

namespace EnduranceJudge.Domain.State.Performances
{
    public class Performance : DomainObjectBase<PerformanceException>, IPerformanceState
    {
        private Performance() {}
        public Performance(Phase phase, DateTime startTime) : base(GENERATE_ID)
        {
            this.Phase = phase;
            this.IsAnotherInspectionRequired = phase.RequireCompulsoryInspection;
            this.StartTime = startTime;
        }

        public Phase Phase { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? InspectionTime { get; set; }
        public DateTime? ReInspectionTime { get; set; }
        public bool IsAnotherInspectionRequired { get; internal set; }
        public DateTime? RequiredInspectionTime { get; set; }
        public DateTime? NextPerformanceStartTime { get; internal set; }
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
