using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Rankings.Aggregates;

public class RankList : IAggregate, IEnumerable<Participation>
{
    private readonly IEnumerable<Participation> participants;

    internal RankList(Category category, IEnumerable<Participation> participations)
    {
        if (category == default)
        {
            throw new InvalidOperationException("RankList category cannot be 'Invalid'");
        }

        this.Category = category;
        this.participants = this.Rank(category, participations);
    }

    public Category Category { get; }

    private IEnumerable<Participation> Rank(Category category, IEnumerable<Participation> participants)
    {
        var ranked = category == Category.Kids
            ? this.RankKids(participants)
            : this.RankAdults(participants);
        return ranked;
    }

    private IEnumerable<Participation> RankKids(IEnumerable<Participation> kids)
        => kids
            .Select(this.CalculateTotalRecovery)
            .OrderByDescending(tuple => tuple.Item1)
            .Select(tuple => tuple.Item2)
            .ToList();

    private IEnumerable<Participation> RankAdults(IEnumerable<Participation> adults)
        => adults
            .OrderByDescending(participation => participation.Participant
                .LapRecords
                .All(performance => performance.Result?.IsRanked ?? false))
            .ThenBy(participation => participation.Participant
                .LapRecords
                .Last()
                .ArrivalTime);

    private (TimeSpan, Participation) CalculateTotalRecovery(Participation participation)
    {
        var totalRecovery = participation.Participant
            .LapRecords
            .Where(x => x.Result != null)
            .Aggregate(
                TimeSpan.Zero,
                (total, x) => total + this.GetRecoveryTime(x));

        return (totalRecovery, participation);
    }

    private TimeSpan GetRecoveryTime(LapRecord record)
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


    public IEnumerator<Participation> GetEnumerator() => this.participants.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}