using EnduranceJudge.Domain.State.Participants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.Branches.StartList;

public class StartList : List<StartModel>
{
    internal StartList(IEnumerable<Participant> participants)
    {
        foreach (var participant in participants)
        {
            this.AddStart(participant);
        }
    }

    private void AddStart(Participant participant)
    {
        //TODO: Refactor this
        var performances = participant.Participation.Performances
            .Skip(1)
            .Where(x => x.Result != null)
            .Select(performance => new StartModel
            {
                Number = participant.Number,
                Name = participant.Name,
                CountryName = participant.Athlete.Country.Name,
                Distance = participant.Participation.Distance,
                StartTime = performance.NextPerformanceStartTime!.Value,
                HasStarted = performance.StartTime < DateTime.Now,
            });
        this.AddRange(performances);
    }
}
