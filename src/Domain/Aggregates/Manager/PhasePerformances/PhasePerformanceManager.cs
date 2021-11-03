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
            if (this.performance.Result != null)
            {
                this.Throw<PhasePerformanceException>(IS_COMPLETE);
            }

            if (this.performance.ArrivalTime == null)
            {
                this.performance.ArrivalTime = time;
            }
            else if (this.performance.InspectionTime == null)
            {
                this.performance.InspectionTime = time;
            }
            else if (this.performance.ReInspectionTime == null)
            {
                this.performance.ReInspectionTime = time;
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
    }
}
