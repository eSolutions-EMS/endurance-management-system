using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Results;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.State.Performances
{
    public class Performance : DomainBase<PerformanceException>, IPerformanceState
    {
        private readonly List<TimeSpan> previousTimes = new();
        private readonly List<double> previousLengths = new();

        private Performance() {}
        public Performance(
            Phase phase,
            DateTime startTime,
            IEnumerable<double> previousLengths,
            IEnumerable<TimeSpan> previousTimes) : base(GENERATE_ID)
        {
            this.previousLengths = previousLengths.ToList();
            this.previousTimes = previousTimes.ToList();
            this.Phase = phase;
            this.StartTime = startTime;
        }

        public Phase Phase { get; internal set; }
        public DateTime StartTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? InspectionTime { get; set; }
        public DateTime? ReInspectionTime { get; set; }
        public bool IsReInspectionRequired { get; internal set; }
        public bool IsRequiredInspectionRequired { get; internal set; }
        public DateTime? RequiredInspectionTime { get; set; } // TODO: internal-set after testing phase
        public DateTime? CompulsoryRequiredInspectionTime { get; internal set; }
        public DateTime? NextPerformanceStartTime { get; internal set; }
        public double LengthSoFar
            => this.previousLengths.Aggregate(0d, (sum, leng) => sum + leng) + this.Phase.LengthInKm;
        public Result Result { get; internal set; }

        public TimeSpan? RecoverySpan
            => (this.ReInspectionTime ?? this.InspectionTime) - this.ArrivalTime;
        public TimeSpan? Time
            => this.Phase.IsFinal
                ? (this.ReInspectionTime ?? this.InspectionTime) - this.StartTime
                : this.ArrivalTime - this.StartTime;
        public double? AverageSpeed
        {
            get
            {
                var phaseLengthInKm = this.Phase.LengthInKm;
                var totalHours = this.Time?.TotalHours;
                return  phaseLengthInKm / totalHours;
            }
        }
        public double? AverageSpeedTotal
        {
            get
            {
                if (!this.AverageSpeed.HasValue)
                {
                    return null;
                }
                var totalLengths = this.previousLengths.Aggregate(0d, (sum, leng) => sum + leng)
                    + this.Phase.LengthInKm;
                var totalHours = this.previousTimes.Aggregate(0d, (sum, span) => sum + span.TotalHours)
                    + this.Time!.Value.TotalHours;
                var totalAverageSpeed = totalLengths / totalHours;
                return totalAverageSpeed;
            }
        }
    }
}
