using System.Collections.Generic;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public class StartList
{
    static readonly TimeSpan START_EXPIRY_TIME = TimeSpan.FromMinutes(15);

    public StartList(){ }

    public List<Start> Starts { get; set; } = [];

    public List<Start> History
    {
        get 
        {
            var now = DateTime.Now.TimeOfDay;
            return Starts
                .Where(s=> now - s.StartAt > START_EXPIRY_TIME)
                .OrderBy(s=>s.StartAt)
                .ToList();
        }
    }

    public List<Start> Upcoming
    {
        get
        {
            var now = DateTime.Now.TimeOfDay;
            return Starts
                .Where(s=> now - s.StartAt <= START_EXPIRY_TIME)
                .OrderBy(s => s.StartAt)
                .ToList();
        }
    }

    public void AssignStarts(IEnumerable<Participation> participations)
    {
        foreach (var participation in participations)
        {
            if (!participation.IsEliminated())
            {
                var phases = participation.Phases;
                var now = new Timestamp(DateTime.Now);
                foreach (var phase in phases.Where(p => p.StartTime != null))
                {
                    var phaseIndex = phases.IndexOf(phase);
                    var start = new Start(
                        participation.Combination.Name,
                        participation.Combination.Number,
                        phaseIndex + 1,
                        phases[phaseIndex].Length,
                        participation.Phases.Distance,
                        phase.StartTime!.DateTime.TimeOfDay
                    );
                    Starts.Add(start);
                }
            }
        }
    }

    public void Add(Start start)
    {   
        Starts.Add(start);
    }
}
