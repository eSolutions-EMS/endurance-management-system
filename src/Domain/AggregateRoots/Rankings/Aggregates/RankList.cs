using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.TimeRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Rankings.Aggregates
{
    public class RankList : IAggregate, IEnumerable<Participant>
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
                    .TimeRecords
                    .All(performance => performance.Result?.IsRanked ?? false))
                .ThenBy(participant => participant
                    .TimeRecords
                    .Last()
                    .ArrivalTime);

        private (TimeSpan, Participant) CalculateTotalRecovery(Participant participant)
        {
            var totalRecovery = participant
                .TimeRecords
                .Where(x => x.Result != null)
                .Aggregate(
                    TimeSpan.Zero,
                    (total, x) => total + this.GetRecoveryTime(x));

            return (totalRecovery, participant);
        }

        private TimeSpan GetRecoveryTime(TimeRecord record)
        {
            var arrival = record.ArrivalTime;
            var inspection = record.ReInspectionTime ?? record.InspectionTime;
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
