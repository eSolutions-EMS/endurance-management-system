using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Manager.DTOs;
using EnduranceJudge.Domain.State.PhasePerformances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.PhaseResults;
using System;

namespace EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances
{
    public class PhasePerformanceManager : DomainObjectBase<PhasePerformanceObjectException>, IPhasePerformanceState
    {
        private static readonly string ArrivalTimeIsNullMessage = $"cannot complete: ArrivalTime cannot be null.";
        private static readonly string InspectionTimeIsNullMessage = $"cannot complete: InspectionTime cannot be null";

        internal PhasePerformanceManager(PhaseDto phase, DateTime startTime)
        {
            this.Validate(() =>
            {
                this.StartTime = startTime.IsRequired(nameof(startTime));
                phase.IsRequired(nameof(phase));
            });
            this.PhaseId = phase.Id;
            this.PhaseOrderBy = phase.OrderBy;
            this.PhaseLengthInKm = phase.LengthInKm;
        }

        public DateTime StartTime { get; private set; }
        public DateTime? ArrivalTime { get; private set; }
        public DateTime? InspectionTime { get; private set; }
        public DateTime? ReInspectionTime { get; private set; }
        public PhaseResult PhaseResult { get; private set; }

        public int PhaseId { get; }
        public int PhaseOrderBy { get; }
        public int PhaseLengthInKm { get; }
        public int MaxRecoveryTimeInMinutes { get; set; }
        public int RestTimeInMinutes { get; set; }

        public TimeSpan? LoopSpan => this.ArrivalTime - this.StartTime;
        public TimeSpan? PhaseSpan => (this.ReInspectionTime ?? this.InspectionTime) - this.StartTime;
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
        public bool IsComplete => this.PhaseResult != null;
        public bool CanArrive
            => !this.IsComplete
                && !this.ArrivalTime.HasValue;
        public bool CanInspect
            => !this.IsComplete
                && this.ArrivalTime.HasValue
                && !this.InspectionTime.HasValue;
        public bool CanReInspect
            => !this.IsComplete
                && this.InspectionTime.HasValue
                && !this.ReInspectionTime.HasValue;
        public bool CanComplete
            => !this.IsComplete
                && (this.ReInspectionTime.HasValue || this.InspectionTime.HasValue);

        internal void Arrive(DateTime time)
            => this.Validate(() =>
            {
                if (!this.CanArrive)
                {
                    return;
                }
                this.ArrivalTime = time.IsRequired(nameof(time));
            });
        internal void Inspect(DateTime time)
            => this.Validate(() =>
            {
                if (!this.CanInspect)
                {
                    return;
                }
                this.InspectionTime = time.IsRequired(nameof(time));
            });
        internal void ReInspect(DateTime time)
            => this.Validate(() =>
            {
                if (!this.CanReInspect)
                {
                    return;
                }
                this.ReInspectionTime = time.IsRequired(nameof(time));
            });
        internal void CompleteSuccessful()
        {
            this.ArrivalTime.IsNotDefault(ArrivalTimeIsNullMessage);
            this.InspectionTime.IsNotDefault(InspectionTimeIsNullMessage);
            this.PhaseResult = new PhaseResult();
        }
        internal void CompleteUnsuccessful(string code)
        {
            this.PhaseResult = new PhaseResult(code);
        }

        private double GetAverageSpeed(TimeSpan timeSpan)
        {
            var phaseLengthInKm = this.PhaseLengthInKm;
            var totalHours = timeSpan.TotalHours;
            return  phaseLengthInKm / totalHours;
        }
    }
}
