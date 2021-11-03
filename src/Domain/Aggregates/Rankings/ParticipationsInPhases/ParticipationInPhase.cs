using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Rankings.DTOs;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.PhaseResults;
using System;

namespace EnduranceJudge.Domain.Aggregates.Rankings.PhasePerformances
{
    public class PhasePerformance : DomainObjectBase<RankingPhasePerformanceObjectException>
    {
        private PhasePerformance()
        {
        }

        public DateTime ArrivalTime { get; private set; }
        public DateTime InspectionTime { get; private set; }
        public DateTime? ReInspectionTime { get; private set; }
        public PhaseResult PhaseResult { get; private set; }
        public PhaseForRanking Phase { get; private set; }

        public TimeSpan RecoverySpan
        {
            get
            {
                this.Validate(() =>
                {
                    this.ArrivalTime.IsRequired(nameof(this.ArrivalTime));
                    this.InspectionTime.IsRequired(nameof(this.ArrivalTime));
                });

                var inspectionTime = this.ReInspectionTime ?? this.InspectionTime;
                return this.ArrivalTime - inspectionTime;
            }
        }
        public bool IsNotComplete => this.PhaseResult == null;
    }
}
