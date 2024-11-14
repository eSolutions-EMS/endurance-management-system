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
            var now = DateTime.Now.TimeOfDay;
            return Starts
                .Where(s => now - s.Time.TimeOfDay > START_EXPIRY_TIME)
                .OrderBy(s => s.Time)
                .ToList();
        }
    }

    public IReadOnlyList<Start> Upcoming
    {
        get
        {
            var now = DateTime.Now.TimeOfDay;
            return Starts
                .Where(s => now - s.Time.TimeOfDay <= START_EXPIRY_TIME)
                .OrderBy(s => s.Time)
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
                foreach (var phase in phases)
                {
                    if (phase.StartTime == null)
                    {
                        continue;
                    }
                    var zeroBasedPhaseIndex = phases.IndexOf(phase);
                    var phaseIndex = zeroBasedPhaseIndex + 1;
                    var start = new Start(
                        participation.Combination.Name,
                        participation.Combination.Number,
                        phaseIndex,
                        phases[zeroBasedPhaseIndex].Length,
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
}
