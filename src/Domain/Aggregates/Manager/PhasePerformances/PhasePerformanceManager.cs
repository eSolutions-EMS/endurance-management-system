using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State.PhasePerformances;
using EnduranceJudge.Domain.State.PhaseResults;
using EnduranceJudge.Domain.State.Phases;
using System;
using static EnduranceJudge.Localization.Strings.Domain.Manager.Participation;

namespace EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances
{
    public class PhasePerformanceManager : ManagerObjectBase
    {
        private readonly PhasePerformance performance;
        private const string ARRIVAL_TIME_IS_NULL_MESSAGE = "cannot complete: ArrivalTime cannot be null.";
        private const string INSPECTION_TIME_IS_NULL_MESSAGE = "cannot complete: InspectionTime cannot be null";

        internal PhasePerformanceManager(PhasePerformance performance)
        {
            this.performance = performance;
            this.Phase = performance.Phase;
        }

        public Phase Phase { get; }

        public bool IsComplete
            => this.performance.Result != null;
        public DateTime VetGatePassedTime =>
            this.performance.ReInspectionTime ?? this.performance.InspectionTime!.Value;

        internal void Update(DateTime time)
        {
            this.Validate<PhasePerformanceException>(() =>
            {
                time.IsRequired(nameof(time));
            });
            if (this.IsComplete)
            {
                this.Throw<PhasePerformanceException>(IS_COMPLETE);
            }
            if (this.performance.InspectionTime.HasValue && this.performance.ReInspectionTime.HasValue)
            {
                this.Throw<PhasePerformanceException>(CAN_ONLY_BE_COMPLETED);
            }

            if (this.performance.ArrivalTime == null)
            {
                this.Arrive(time);
            }
            else if (this.performance.InspectionTime == null)
            {
                this.Inspect(time);
            }
            else if (this.performance.ReInspectionTime == null)
            {
                this.ReInspect(time);
            }
        }
        internal void Complete()
        {
            this.Validate<PhasePerformanceException>(() =>
            {
                this.performance.ArrivalTime.IsRequired(ARRIVAL_TIME_IS_NULL_MESSAGE);
                this.performance.InspectionTime.IsRequired(INSPECTION_TIME_IS_NULL_MESSAGE);
            });

            this.performance.Result = new PhaseResult();
        }
        internal void CompleteUnsuccessful(string code)
        {
            this.performance.Result = new PhaseResult(code);
        }

        private void Arrive(DateTime time)
        {
            if (time <= this.performance.StartTime)
            {
                var message = string.Format(
                    DATE_TIME_HAS_TO_BE_LATER_TEMPLATE,
                    nameof(this.performance.ArrivalTime),
                    nameof(this.performance.StartTime));
                this.Throw<PhasePerformanceException>(message);
            }

            this.performance.ArrivalTime = time;
        }

        private void Inspect(DateTime time)
        {
            if (time <= this.performance.ArrivalTime)
            {
                var message = string.Format(
                    DATE_TIME_HAS_TO_BE_LATER_TEMPLATE,
                    nameof(this.performance.InspectionTime),
                    nameof(this.performance.ArrivalTime));
                this.Throw<PhasePerformanceException>(message);
            }

            this.performance.InspectionTime = time;
        }

        private void ReInspect(DateTime time)
        {
            if (time <= this.performance.InspectionTime)
            {
                var message = string.Format(
                    DATE_TIME_HAS_TO_BE_LATER_TEMPLATE,
                    nameof(this.performance.ReInspectionTime),
                    nameof(this.performance.InspectionTime));
                this.Throw<PhasePerformanceException>(message);
            }

            this.performance.ReInspectionTime = time;
        }
    }
}
