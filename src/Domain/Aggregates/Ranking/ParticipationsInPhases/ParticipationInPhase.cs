using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Manager.ResultsInPhases;
using EnduranceJudge.Domain.Aggregates.Ranking.DTOs;
using EnduranceJudge.Domain.Core.Models;
using System;

namespace EnduranceJudge.Domain.Aggregates.Ranking.ParticipationsInPhases
{
    public class ParticipationInPhase : DomainBase<RankingParticipationInPhaseException>
    {
        private ParticipationInPhase()
        {
        }

        public DateTime ArrivalTime { get; }
        public DateTime InspectionTime { get; }
        public DateTime? ReInspectionTime { get; }
        public ResultInPhase Result { get; private set; }
        public PhaseForRanking Phase { get; private set; }

        public TimeSpan RecoverySpan
        {
            get
            {
                this.Validate(() =>
                {
                    this.ArrivalTime.IsNotDefault(nameof(this.ArrivalTime));
                    this.InspectionTime.IsNotDefault(nameof(this.ArrivalTime));
                });

                var inspectionTime = this.ReInspectionTime ?? this.InspectionTime;
                return this.ArrivalTime - inspectionTime;
            }
        }
    }
}
