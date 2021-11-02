using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Rankings.PhasePerformances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Rankings.Participations
{
    public class Participation : DomainObjectBase<RankingParticipationObjectException>
    {
        private const string NOT_RANKED_MESSAGE = "cannot be classified as they are not qualified for ranking.";

        internal Participation()
        {
        }

        public int Number { get; private init; }
        public string AthleteFirstName { get; private init; }
        public string AthleteLastName { get; private init; }
        public string HorseName { get; private init; }
        public Category Category { get; private init; }
        public IReadOnlyList<PhasePerformance> PhasePerformances { get; private init; }

        public bool IsNotComplete
            => this.PhasePerformances.Any(pip => pip.IsNotComplete);
        public bool IsQualified
            => this.PhasePerformances.All(participation => participation.PhaseResult.IsRanked);

        public DateTime FinalTime
        {
            get
            {
                this.IsQualified.IsNotDefault(NOT_RANKED_MESSAGE);

                var finalPhase = this.PhasePerformances.Single(participation => participation.Phase.IsFinal);
                return finalPhase.ArrivalTime;
            }
        }

        public long RecoverySpanInTicks
        {
            get
            {
                this.IsQualified.IsNotDefault(NOT_RANKED_MESSAGE);

                var recoverySum = this.PhasePerformances.Aggregate(
                    TimeSpan.Zero,
                    (sum, participation) => sum.Add(participation.RecoverySpan));

                return recoverySum.Ticks;
            }
        }
    }
}
