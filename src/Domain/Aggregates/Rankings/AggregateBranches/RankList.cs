using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.PhasePerformances;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches
{
    public class RankList : ManagerObjectBase, IEnumerable<Participant>
    {
        private readonly IEnumerable<Participant> participants;

        internal RankList(Category category, IEnumerable<Participant> participants)
        {
            if (category == default)
            {
                throw new InvalidOperationException("RankList category cannot be 'Invalid'");
            }

            this.Category = category;
            this.participants = this.Rank(category, participants);
        }

        public Category Category { get; }

        private IEnumerable<Participant> Rank(Category category, IEnumerable<Participant> participants)
        {
            var ranked = category == Category.Kids
                ? this.RankKids(participants)
                : this.RankAdults(participants);
            return ranked;
        }

        private IEnumerable<Participant> RankKids(IEnumerable<Participant> kids)
            => kids
                .Select(this.CalculateTotalRecovery)
                .OrderByDescending(tuple => tuple.Item1)
                .Select(tuple => tuple.Item2)
                .ToList();

        private IEnumerable<Participant> RankAdults(IEnumerable<Participant> adults)
            => adults
                .OrderByDescending(participant => participant
                    .Participation
                    .Performances
                    .All(performance => performance.Result?.IsRanked ?? false))
                .ThenBy(participant => participant
                    .Participation
                    .Performances
                    .FirstOrDefault(performance => performance.Phase.IsFinal)
                    ?.ArrivalTime);

        private (TimeSpan, Participant) CalculateTotalRecovery(Participant participant)
        {
            var totalRecovery = participant
                .Participation
                .Performances
                .Where(x => x.Result != null)
                .Aggregate(
                    TimeSpan.Zero,
                    (total, x) => total + this.GetRecoveryTime(x));

            return (totalRecovery, participant);
        }

        private TimeSpan GetRecoveryTime(PhasePerformance performance)
        {
            var arrival = performance.ArrivalTime;
            var inspection = performance.ReInspectionTime ?? performance.InspectionTime;
            if (!arrival.HasValue || !inspection.HasValue)
            {
                return TimeSpan.Zero;
            }
            var recovery = inspection - arrival;
            return recovery.Value;
        }


        public IEnumerator<Participant> GetEnumerator() => this.participants.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
