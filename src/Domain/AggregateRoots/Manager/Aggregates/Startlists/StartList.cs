using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class Startlist
{
    internal Startlist(IEnumerable<Participation> participations, bool includePast)
    {
        var entries = new List<StartModel>();
        foreach (var participant in participations.Where(x =>
                     x.Participant.LapRecords.Any(y => y.NextStarTime.HasValue)))
        {
            entries.AddRange(this.AddEntries(participant, includePast));
        }
        this.List = entries
            .OrderByDescending(x => x.StartTime > DateTime.Now)
            .ThenBy(x => x.StartTime)
            .ToList();
    }

    public List<StartModel> List = new();

    private IEnumerable<StartModel> AddEntries(Participation participation, bool includePast)
    {
        var performances = Performance.GetAll(participation).ToList();
        if (includePast)
        {
            foreach (var record in performances.Where(x => x.NextStartTime.HasValue))
            {
                yield return this.CreateModel(participation, record.NextStartTime!.Value);
            }
        }
        else
        {
            var performance = performances.Last(x => x.NextStartTime.HasValue);
            yield return this.CreateModel(participation, performance.NextStartTime!.Value);
        }
    }

    private StartModel CreateModel(Participation participation, DateTime startTime)
        =>  new()
        {
            Number = participation.Participant.Number,
            Name = participation.Participant.Name,
            CountryName = participation.Participant.Athlete.Country.Name,
            Distance = participation.Distance!.Value,
            StartTime = startTime,
        };
}
