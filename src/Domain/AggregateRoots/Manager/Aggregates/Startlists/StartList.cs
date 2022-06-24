using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class Startlist : SortedSet<StartModel>
{
    internal Startlist(IEnumerable<Participation> participation, bool includePast) : base(new StartlistComparer())
    {
        foreach (var participant in participation)
        {
            this.Handle(participant, includePast);
        }
    }

    private void Handle(Participation participation, bool includePast)
    {
        var performances = Performance.GetAll(participation);
        var upcoming = performances.FirstOrDefault(x => x.NextStartTime > DateTime.Now);
        if (upcoming != null)
        {
            this.AddStart(participation, upcoming.NextStartTime!.Value);
        }
        if (includePast)
        {
            foreach (var record in participation.Participant.LapRecords.Where(x => x.StartTime < DateTime.Now))
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

public class StartlistComparer : IComparer<StartModel>
{
    public int Compare(StartModel x, StartModel y)
    {
        if (ReferenceEquals(null, y))
            return 1;
        if (ReferenceEquals(null, x))
            return -1;
        if (x.StartTime > y.StartTime)
            return -1;
        if (x.StartTime < y.StartTime)
            return 1;
        return 1;
        // return x.Number > y.Number ? 1 : -1;
    }
}
