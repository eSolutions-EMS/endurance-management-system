using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;

// TODO: Rename to Ranklist
public class RanklistAggregate : List<Participation>, IAggregate
{
    internal RanklistAggregate(Category category, IEnumerable<Participation> participations)
    {
        if (category == default)
        {
            throw new InvalidOperationException("RankList category cannot be 'Invalid'");
        }

        this.Category = category;
        var ranklist = this.Rank(category, participations);
        this.AddRange(ranklist);
    }

    public Category Category { get; }

    private IEnumerable<Participation> Rank(Category category, IEnumerable<Participation> participants)
    {
        var ranked = category == Category.Kids
            ? this.RankKids(participants)
            : this.RankAdults(participants);
        return ranked;
    }

    private IEnumerable<Participation> RankKids(IEnumerable<Participation> participations)
        => participations
            .Where(x => x.Participant.Athlete.Category == Category.Kids)
            .Select(this.CalculateTotalRecovery)
            .OrderBy(x => this.IsNotQualifiedPredicate(x.Item2))
            .ThenByDescending(x => x.Item1)
            .Select(x => x.Item2)
            .ToList();

    private IEnumerable<Participation> RankAdults(IEnumerable<Participation> participations)
        => participations
            .Where(x => x.Participant.Athlete.Category == Category.Adults)
            .OrderBy(this.IsNotQualifiedPredicate)
            .ThenBy(participation => participation.Participant
                .LapRecords
                .LastOrDefault()
                ?.ArrivalTime);

    private Func<Participation, bool> IsNotQualifiedPredicate
        => participation => participation.Participant
            .LapRecords
            .Any(performance => performance.Result?.IsNotQualified ?? true);

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
}
