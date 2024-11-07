using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public class StartList
{
    public StartList(IEnumerable<Participation> participations)
    {
        AssignStarts(participations);
    }
    public List<Start> Upcoming { get; private set; } = new List<Start>();
    public List<Start> History { get; private set; } = new List<Start>();

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
                    var start = new Start(participation.Combination.Name, participation.Combination.Number, phaseIndex + 1, phases[phaseIndex].Length, participation.Phases.Distance, phase.StartTime!);
                    if (now - phase.StartTime > TimeSpan.FromMinutes(15))
                    {
                        if(!History.Contains(start)) History.Add(start);
                    }
                    else
                    {
                        if(!Upcoming.Contains(start)) Upcoming.Add(start);
                    }
                }
            }
        }
    }
}
