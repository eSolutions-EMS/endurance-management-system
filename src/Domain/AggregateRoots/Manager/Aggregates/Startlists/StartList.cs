using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class Startlist : List<StartModel>
{
    internal Startlist(IEnumerable<Participation> participation, bool includePast)
    {
        foreach (var participant in participation)
        {
            this.Handle(participant, includePast);
        }
    }

    private void Handle(Participation participation, bool includePast)
    {
        var performances = Performance.GetAll(participation);
        if (!includePast)
        {
            var current = performances.FirstOrDefault(x => x.NextStartTime > DateTime.Now);
            if (current == null)
            {
                return;
            }
            this.AddStart(participation, current.NextStartTime!.Value);
        }
        else
        {
            foreach (var record in participation.Participant.LapRecords.Where(x => x.Result != null))
            {
                this.AddStart(participation, record.StartTime);
            }
        }
    }

    private void AddStart(Participation participation, DateTime startTime)
    {
        var start = new StartModel
        {
            Number = participation.Participant.Number,
            Name = participation.Participant.Name,
            CountryName = participation.Participant.Athlete.Country.Name,
            Distance = participation.Distance!.Value,
            StartTime = startTime,
            HasStarted = startTime < DateTime.Now,
        };
        this.Add(start);
    }
}
