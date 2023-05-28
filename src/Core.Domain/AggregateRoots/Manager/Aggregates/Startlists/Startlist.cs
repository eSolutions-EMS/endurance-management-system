using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class Startlist
{
    internal Startlist(IEnumerable<Participation> participations, bool includePast)
    {
        var entries = new List< StartModel>();
        foreach (var participant in participations.Where(x =>
                     x.Participant.LapRecords.Any(y => y.NextStarTime.HasValue)
                     && !x.Participant.LapRecords.Any(y => y.Result == null || y.Result.IsNotQualified)))
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
                yield return CreateModel(participation, record.NextStartTime!.Value);
            }
        }
        else
        {
            var performance = performances.Last(x => x.NextStartTime.HasValue);
            yield return CreateModel(participation, performance.NextStartTime!.Value);
        }
    }

    public static StartModel? CreateModel(Participation participation, DateTime? startTime = null)
    {
        if (participation.Participant.LapRecords.Last()?.Lap.IsFinal ?? false)
        {
            return null;
        }
        if (startTime == null)
        {
            startTime = participation.Participant.LapRecords.Last().NextStarTime;
        }
        return new()
		{
			Number = participation.Participant.Number,
			Name = participation.Participant.Name,
			AthleteName = participation.Participant.Athlete.Name,
			CountryName = participation.Participant.Athlete.Country.Name,
			Distance = participation.Distance!.Value,
			StartTime = startTime.Value,
		};
	}
        
}
