using System.Collections.Generic;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public class StartList
{
    static readonly TimeSpan START_EXPIRY_TIME = TimeSpan.FromMinutes(15);

    public StartList() { }

    public List<Start> Starts { get; set; } = [];

    public IReadOnlyList<Start> History 
    {
        get
        {
            var history = Starts.Where(s => CurrentTime() - s.Time.TimeOfDay > START_EXPIRY_TIME);
            return OrderByTime(history);
        }
    }
 

    public IReadOnlyList<Start> Upcoming
    {
        get
        {
            var upcoming = Starts.Where(s => CurrentTime() - s.Time.TimeOfDay <= START_EXPIRY_TIME);
            return OrderByTime(upcoming);
        }
    }

    public void AssignStarts(IEnumerable<Participation> participations)
    {
        foreach (var participation in participations)
        {
            if (!participation.IsEliminated())
            {
                var phases = participation.Phases;
                foreach (var phase in phases)
                {
                    if (phase.StartTime == null)
                    {
                        continue;
                    }
                    var phaseIndex = phases.IndexOf(phase);
                    var phaseNumber = phaseIndex + 1;
                    var start = new Start(
                        participation.Combination.Name,
                        participation.Combination.Number,
                        phaseNumber,
                        phases[phaseIndex].Length,
                        participation.Phases.Distance,
                        phase.StartTime.DateTime.DateTime
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

    TimeSpan CurrentTime()
    {
        return DateTimeOffset.Now.TimeOfDay;
    }

    List<Start> OrderByTime(IEnumerable<Start> starts)
    {
        return starts.OrderBy(s => s.Time).ToList();
    }
}
