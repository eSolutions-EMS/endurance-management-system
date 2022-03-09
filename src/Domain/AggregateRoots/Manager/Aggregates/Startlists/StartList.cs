using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.TimeRecords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

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
        var performances = participant.TimeRecords;
        if (!includePast)
        {
            var current = performances.FirstOrDefault(x => x.StartTime > DateTime.Now);
            if (current == null)
            {
                return;
            }
            this.AddStart(participant, current);
        }
        else
        {
            foreach (var record in participant.TimeRecords.Where(x => x.Result != null))
            {
                this.AddStart(participant, record);
            }
        }
    }

    private void AddStart(Participant participant, TimeRecord record)
    {
        var start = new StartModel
        {
            Number = participant.Number,
            Name = participant.Name,
            CountryName = participant.Athlete.Country.Name,
            Distance = participant.Participation.Distance!.Value,
            StartTime = record.StartTime,
            HasStarted = record.StartTime < DateTime.Now,
        };
        this.Add(start);
    }
}
