using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.Aggregates.Manager.DTOs;
using EnduranceJudge.Domain.Aggregates.Manager.ResultsInPhases;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.States;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInPhases
{
    public class ParticipationInPhase : DomainBase<ParticipationInPhaseException>, IParticipationInPhaseState
    {
        private static readonly string ArrivalTimeIsNullMessage = $"cannot complete: ArrivalTime cannot be null.";
        private static readonly string InspectionTimeIsNullMessage = $"cannot complete: InspectionTime cannot be null";

        private ParticipationInPhase()
        {
        }

        internal ParticipationInPhase(PhaseDto phase, DateTime startTime)
        {
            this.Validate(() =>
            {
                this.StartTime = startTime
                    .IsRequired(nameof(startTime))
                    .HasDatePassed();

                phase.IsRequired(nameof(phase));
            });
            this.LengthInKm = phase.LengthInKm;
            this.PhasesForCategories = phase.PhasesForCategories;
        }

        public DateTime StartTime { get; private set; }
        public DateTime? ArrivalTime { get; private set; }
        public DateTime? InspectionTime { get; private set; }
        public DateTime? ReInspectionTime { get; private set; }
        public ResultInPhase ResultInPhase { get; private set; }

        public int OrderBy { get; private init; }
        public int LengthInKm { get; private init; }
        public IEnumerable<PhaseForCategoryDto> PhasesForCategories { get; private init; }

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
        public bool IsComplete => this.ResultInPhase != null;

        internal void Arrive(DateTime time)
        {
            this.ArrivalTime = time.IsRequired(nameof(time));
        }
        internal void Inspect(DateTime time)
        {
            this.InspectionTime = time.IsRequired(nameof(time));
        }
        internal void ReInspect(DateTime time)
        {
            this.ReInspectionTime = time.IsRequired(nameof(time));
        }

        internal void CompleteSuccessful()
        {
            var successfulResult = new ResultInPhase();
            this.Complete(successfulResult);
        }
        internal void CompleteUnsuccessful(string code)
        {
            var unsuccessfulResult = new ResultInPhase(code);
            this.Complete(unsuccessfulResult);
        }

        private void Complete(ResultInPhase result)
            => this.Validate(() =>
            {
                this.ArrivalTime.IsNotDefault(ArrivalTimeIsNullMessage);
                this.InspectionTime.IsNotDefault(InspectionTimeIsNullMessage);
                this.ResultInPhase = result.IsRequired(nameof(result));
            });
        private double GetAverageSpeed(TimeSpan timeSpan)
        {
            var phaseLengthInKm = this.LengthInKm;
            var totalHours = timeSpan.TotalHours;
            return  phaseLengthInKm / totalHours;
        }
    }
}
