using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.Results;
using EnduranceJudge.Domain.State.Phases;
using System;
using static EnduranceJudge.Localization.Strings;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates
{
    public class PerformancesAggregate : IAggregate
    {
        public const int COMPULSORY_INSPECTION_TIME_BEFORE_NEXT_START = 15;
        private readonly Performance performance;
        private readonly Validator<PerformanceException> validator;

        internal PerformancesAggregate(Performance performance)
        {
            this.performance = performance;
            this.Phase = performance.Phase;
            this.validator = new Validator<PerformanceException>();
        }

        public Phase Phase { get; }

        public DateTime VetGatePassedTime =>
            this.performance.ReInspectionTime ?? this.performance.InspectionTime!.Value;

        internal void Update(DateTime time)
        {
            // TODO: Probably throw error as well
            this.validator.IsRequired(time, nameof(time));

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
        internal void Complete(string code)
        {
            this.performance.Result = new Result(code);
        }
        internal void ReInspection(bool isRequired)
        {
            this.performance.IsReInspectionRequired = isRequired;
        }
        internal void RequireInspection(bool isRequired)
        {
            if (this.Phase.IsCompulsoryInspectionRequired)
            {
                throw Helper.Create<PerformanceException>(REQUIRED_INSPECTION_IS_NOT_ALLOWED_MESSAGE);
            }
            this.performance.IsRequiredInspectionRequired = isRequired;
        }
        internal void Edit(IPerformanceState state)
        {
            if (state.ArrivalTime.HasValue && this.performance.ArrivalTime != state.ArrivalTime)
            {
                if (this.performance.ArrivalTime == null)
                {
                    throw Helper.Create<PerformanceException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, ARRIVAL_TERM);
                }
                this.Arrive(state.ArrivalTime.Value);
            }
            if (state.InspectionTime.HasValue && this.performance.InspectionTime != state.InspectionTime)
            {
                if (this.performance.InspectionTime == null)
                {
                    throw Helper.Create<PerformanceException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, INSPECTION_TERM);
                }
                this.Inspect(state.InspectionTime.Value);
            }
            if (state.ReInspectionTime.HasValue && this.performance.ReInspectionTime != state.ReInspectionTime)
            {
                if (this.performance.ReInspectionTime == null)
                {
                    throw Helper.Create<PerformanceException>(CANNOT_EDIT_PERFORMANCE_MESSAGE, RE_INSPECTION_TERM);
                }
                this.CompleteReInspection(state.ReInspectionTime.Value);
            }
        }

        private void Arrive(DateTime time)
        {
            time = FixDateForToday(time);
            this.validator.IsLaterThan(time, this.performance.StartTime, ARRIVAL_TERM);
            this.performance.ArrivalTime = time;
        }
        private void Inspect(DateTime time)
        {
            time = FixDateForToday(time);
            this.validator.IsLaterThan(time, this.performance.ArrivalTime, INSPECTION_TERM);
            this.performance.InspectionTime = time;
        }
        private void CompleteReInspection(DateTime time)
        {
            time = FixDateForToday(time);
            this.validator.IsLaterThan(time, this.performance.InspectionTime, RE_INSPECTION_TERM);

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
        private void Complete()
        {
            var restTime = this.performance.Phase.RestTimeInMins;
            var nextPhaseStartTime = this.VetGatePassedTime.AddMinutes(restTime);
            if (!this.performance.ArrivalTime.HasValue || !this.performance.InspectionTime.HasValue)
            {
                throw new Exception(PERFORMANCE_INVALID_COMPLETE);
            }
            if (this.performance.IsRequiredInspectionRequired && !this.performance.RequiredInspectionTime.HasValue)
            {
                throw new Exception(PERFORMANCE_INVALID_COMPLETE);
            }
            if (this.Phase.IsCompulsoryInspectionRequired && !this.performance.CompulsoryRequiredInspectionTime.HasValue)
            {
                throw new Exception(PERFORMANCE_INVALID_COMPLETE);
            }

            this.performance.Result = new Result();
            this.performance.NextPerformanceStartTime = nextPhaseStartTime;
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
