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
        var performances = Performance.GetAll(participation).ToList();
        if (includePast)
        {
            foreach (var record in performances.Where(x => x.NextStartTime.HasValue))
            {
                this.AddStart(participation, record.NextStartTime!.Value);
            }
        }
        else
        {
            var performance = performances.Last();
            this.AddStart(participation, performance.NextStartTime!.Value);
        }
    }

    private void AddStart(Participation participation, DateTime startTime)
    {
        var start = new StartModel
        {
            Name = participation.Participant.Name,
            CountryName = participation.Participant.Athlete.Country.Name,
            Distance = participation.Distance!.Value,
            StartTime = startTime,
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
