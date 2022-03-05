using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Performances;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Branches.StartLists;

public class Startlist : List<StartModel>
{
    internal Startlist(IEnumerable<Participant> participants, bool includePast)
    {
        foreach (var participant in participants)
        {
            this.HandleParticipant(participant, includePast);
        }
    }

    private void HandleParticipant(Participant participant, bool includePast)
    {
        var performances = participant.Participation.Performances;
        if (!includePast)
        {
            var current = performances.FirstOrDefault(x => x.NextPerformanceStartTime > DateTime.Now);
            if (current == null)
            {
                return;
            }
            this.AddStart(participant, current);
        }
        else
        {
            foreach (var performance in participant.Participation.Performances.Where(x => x.Result != null))
            {
                this.AddStart(participant, performance);
            }
        }
    }

    private void AddStart(Participant participant, Performance performance)
    {
        var start = new StartModel
        {
            Number = participant.Number,
            Name = participant.Name,
            CountryName = participant.Athlete.Country.Name,
            Distance = participant.Participation.Distance,
            StartTime = performance.NextPerformanceStartTime!.Value,
            HasStarted = performance.StartTime < DateTime.Now,
        };
        this.Add(start);
    }
}
