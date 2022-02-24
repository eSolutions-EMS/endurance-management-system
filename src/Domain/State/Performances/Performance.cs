using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.PhaseResults;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.State.Performances
{
    public class Performance : DomainObjectBase<PerformanceException>, IPerformanceState
    {
        private readonly List<TimeSpan> previousSpans = new();
        private readonly List<double> previousLengths = new();

        private Performance() {}
        public Performance(
            Phase phase,
            DateTime startTime,
            IEnumerable<double> previousLengths,
            IEnumerable<TimeSpan> previousSpans) : base(GENERATE_ID)
        {
            this.previousSpans = previousSpans.ToList();
            this.previousLengths = previousLengths.ToList();
            this.Phase = phase;
            this.IsRequiredInspectionRequired = phase.IsCompulsoryInspectionRequired;
            this.StartTime = startTime;
        }

        public Phase Phase { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? InspectionTime { get; set; }
        public DateTime? ReInspectionTime { get; set; }
        public bool IsReInspectionRequired { get; internal set; }
        public bool IsRequiredInspectionRequired { get; internal set; }
        public DateTime? RequiredInspectionTime { get; set; } // TODO: internal-set after testing phase
        public DateTime? CompulsoryRequiredInspectionTime { get; internal set; }
        public DateTime? NextPerformanceStartTime { get; internal set; }
        public PhaseResult Result { get; internal set; }

        public TimeSpan? RecoverySpan
            => (this.ReInspectionTime ?? this.InspectionTime) - this.ArrivalTime;
        public TimeSpan? Time
            => this.Phase.IsFinal
                ? this.PhaseSpan
                : this.LoopSpan;
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
        public double? AverageSpeedTotalKpH
        {
            get
            {
                if (!this.AverageSpeedForLoopInKpH.HasValue)
                {
                    return null;
                }
                var totalLengths = this.previousLengths.Aggregate(0d, (sum, leng) => sum + leng)
                    + this.Phase.LengthInKm;
                var totalHours = this.previousSpans.Aggregate(0d, (sum, span) => sum + span.TotalHours)
                    + this.LoopSpan!.Value.TotalHours;
                var totalAverageSpeed = totalLengths / totalHours;
                return totalAverageSpeed;
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
        public TimeSpan? LoopSpan
            => this.ArrivalTime - this.StartTime;
        private TimeSpan? PhaseSpan
            => (this.ReInspectionTime ?? this.InspectionTime) - this.StartTime;

        private double GetAverageSpeed(TimeSpan timeSpan)
        {
            var phaseLengthInKm = this.Phase.LengthInKm;
            var totalHours = timeSpan.TotalHours;
            return  phaseLengthInKm / totalHours;
        }
    }
}
