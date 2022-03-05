using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.PhaseResults;
using EnduranceJudge.Domain.State.Phases;
using System;
using static EnduranceJudge.Localization.Translations.Messages;
using static EnduranceJudge.Localization.Translations.Words;
using static EnduranceJudge.Localization.Translations.Terms;

namespace EnduranceJudge.Domain.Aggregates.Manager.Performances
{
    public class PerformanceManager : ManagerObjectBase
    {
        private readonly Performance performance;
        public const int COMPULSORY_INSPECTION_TIME_BEFORE_NEXT_START = 15;

        internal PerformanceManager(Performance performance)
        {
            this.performance = performance;
            this.Phase = performance.Phase;
        }

        public Phase Phase { get; }

        public DateTime VetGatePassedTime =>
            this.performance.ReInspectionTime ?? this.performance.InspectionTime!.Value;

        internal void Update(DateTime time)
        {
            this.Validate<PerformanceException>(() =>
            {
                time.IsRequired(nameof(time));
            });

            if (this.performance.ArrivalTime == null)
            {
                this.Arrive(time);
            }
            else if (this.performance.InspectionTime == null)
            {
                this.Inspect(time);
                if (!this.performance.IsReInspectionRequired
                    && !this.performance.IsRequiredInspectionRequired
                    && !this.Phase.IsCompulsoryInspectionRequired)
                {
                    this.Complete();
                }
            }
            else if (this.performance.IsReInspectionRequired && this.performance.ReInspectionTime == null)
            {
                this.CompleteReInspection(time);
                if (!this.performance.IsRequiredInspectionRequired
                    && !this.Phase.IsCompulsoryInspectionRequired)
                {
                    this.Complete();
                }
            }
            else if (this.performance.IsRequiredInspectionRequired
                        && this.performance.RequiredInspectionTime == null
                    || this.Phase.IsCompulsoryInspectionRequired
                        && this.performance.CompulsoryRequiredInspectionTime == null)
            {
                this.CompleteRequiredInspection();
                this.Complete();
            }
        }
        internal void Complete()
        {
            var restTime = this.performance.Phase.RestTimeInMins;
            var nextPhaseStartTime = this.VetGatePassedTime.AddMinutes(restTime);
            this.Validate<PerformanceException>(() =>
            {
                this.performance.ArrivalTime.IsRequired(ARRIVAL_TIME_IS_NULL_MESSAGE);
                this.performance.InspectionTime.IsRequired(INSPECTION_TIME_IS_NULL_MESSAGE);
                if (this.performance.IsRequiredInspectionRequired)
                {
                    this.performance.RequiredInspectionTime.IsRequired(REQUIRED_INSPECTION_IS_NULL_MESSAGE);
                }
                if (this.Phase.IsCompulsoryInspectionRequired)
                {
                    this.performance.CompulsoryRequiredInspectionTime.IsRequired(
                        COMPULSORY_REQUIRED_INSPECTION_IS_NULL_MESSAGE);
                }
            });

            this.performance.Result = new PhaseResult();
            this.performance.NextPerformanceStartTime = nextPhaseStartTime;
        }
        internal void Complete(string code)
        {
            this.performance.Result = new PhaseResult(code);
        }
        internal void ReInspection(bool isRequired)
        {
            this.performance.IsReInspectionRequired = isRequired;
        }
        internal void RequireInspection(bool isRequired)
        {
            if (this.Phase.IsCompulsoryInspectionRequired)
            {
                this.Throw<PerformanceException>(REQUIRED_INSPECTION_IS_NOT_ALLOWED);
            }
            this.performance.IsRequiredInspectionRequired = isRequired;
        }
        internal void Edit(IPerformanceState state)
        {
            if (state.ArrivalTime.HasValue && this.performance.ArrivalTime != state.ArrivalTime)
            {
                if (this.performance.ArrivalTime == null)
                {
                    this.ThrowRestrictedEdit(ARRIVAL);
                }
                this.Arrive(state.ArrivalTime.Value);
            }
            if (state.InspectionTime.HasValue && this.performance.InspectionTime != state.InspectionTime)
            {
                if (this.performance.InspectionTime == null)
                {
                    this.ThrowRestrictedEdit(INSPECTION);
                }
                this.Inspect(state.InspectionTime.Value);
            }
            if (state.ReInspectionTime.HasValue && this.performance.ReInspectionTime != state.ReInspectionTime)
            {
                if (this.performance.ReInspectionTime == null)
                {
                    this.ThrowRestrictedEdit(RE_INSPECTION);
                }
                this.CompleteReInspection(state.ReInspectionTime.Value);
            }
        }
        private void ThrowRestrictedEdit(string labelName)
        {
            var message = string.Format(CANNOT_EDIT_PERFORMANCE, labelName);
            this.Throw<PerformanceException>(message);
        }
        private void Arrive(DateTime time)
        {
            time = FixDateForToday(time);
            if (time <= this.performance.StartTime)
            {
                var message = string.Format(
                    DATE_TIME_HAS_TO_BE_LATER_TEMPLATE,
                    nameof(this.performance.ArrivalTime),
                    this.performance.StartTime);
                this.Throw<PerformanceException>(message);
            }

            this.performance.ArrivalTime = time;
        }

        private void Inspect(DateTime time)
        {
            time = FixDateForToday(time);
            if (time <= this.performance.ArrivalTime)
            {
                var message = string.Format(
                    DATE_TIME_HAS_TO_BE_LATER_TEMPLATE,
                    nameof(this.performance.InspectionTime),
                    this.performance.ArrivalTime);
                this.Throw<PerformanceException>(message);
            }

            this.performance.InspectionTime = time;
        }

        private void CompleteReInspection(DateTime time)
        {
            time = FixDateForToday(time);
            if (time <= this.performance.InspectionTime)
            {
                var message = string.Format(
                    DATE_TIME_HAS_TO_BE_LATER_TEMPLATE,
                    nameof(this.performance.ReInspectionTime),
                    this.performance.InspectionTime);
                this.Throw<PerformanceException>(message);
            }

            this.performance.ReInspectionTime = time;
        }
        private void CompleteRequiredInspection()
        {
            var inspectionTime = (this.performance.ReInspectionTime ?? this.performance.InspectionTime)
                !.Value
                .AddMinutes(this.Phase.RestTimeInMins)
                .AddMinutes(-COMPULSORY_INSPECTION_TIME_BEFORE_NEXT_START);
            inspectionTime = FixDateForToday(inspectionTime);
            if (this.Phase.IsCompulsoryInspectionRequired)
            {
                this.performance.CompulsoryRequiredInspectionTime = inspectionTime;
            }
            else
            {
                this.performance.RequiredInspectionTime = inspectionTime;
            }
        }

        // TODO: remove after testing phase
        private DateTime FixDateForToday(DateTime date)
        {
            var today = DateTime.Today;
            today = today.AddHours(date.Hour);
            today = today.AddMinutes(date.Minute);
            today = today.AddSeconds(date.Second);
            today = today.AddMilliseconds(date.Millisecond);
            return today;
        }
    }
}
