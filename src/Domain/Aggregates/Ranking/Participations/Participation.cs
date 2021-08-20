using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Ranking.ParticipationsInPhases;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Participations
{
    public class Participation : DomainBase<RankingParticipationException>
    {
        private const string NotRankedMessage = "cannot be classified as they are not qualified for ranking.";

        internal Participation()
        {
        }

        public Category Category { get; private set; }
        public IReadOnlyList<ParticipationInPhase> ParticipationsInPhases { get; private set; }

        public bool IsRanked
            => this.ParticipationsInPhases.All(participation => participation.Result.IsRanked);

        public DateTime FinalTime
        {
            get
            {
                this.IsRanked.IsNotDefault(NotRankedMessage);

                var finalPhase = this.ParticipationsInPhases.Single(participation => participation.Phase.IsFinalPhase);
                return finalPhase.ArrivalTime;
            }
        }

        public long RecoverySpanInTicks
        {
            get
            {
                this.IsRanked.IsNotDefault(NotRankedMessage);

                var recoverySum = this.ParticipationsInPhases.Aggregate(
                    TimeSpan.Zero,
                    (sum, participation) => sum.Add(participation.RecoverySpan));

                return recoverySum.Ticks;
            }
        }
    }
}
