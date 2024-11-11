using NTS.Domain.Core.Entities;
using System.Collections.Generic;

namespace NTS.Domain.Core.Objects;

public class StartList
{
    public StartList()
    {
    }
    public List<Start> Upcoming { get; set; } = new List<Start>();
    public List<Start> History { get; set; } = new List<Start>();

    public void OrderHistoryByAscending()
    {
        History = History.OrderBy(s => s.StartAt).ToList();
    }

    public void Replace(Start start)
    {
        if (History.Contains(start))
        {
            History.Remove(start);
            History.Add(start);
            OrderHistoryByAscending();  
        }
        if (Upcoming.Contains(start)) 
        {
            Upcoming.Remove(start);
            Upcoming.Add(start);
        }
    }

    public void Add(Start start)
    {
        Upcoming.Add(start);
    }

    public void Expire(Start start)
    {
        if (!History.Contains(start))
        {
            Upcoming.Remove(start);
            History.Add(start);
            OrderHistoryByAscending();
        }
    }

    public void AssignStarts(IEnumerable<Participation> participations)
    {
        const int expireUpcomingInMinutes = 15;
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
                    if (now - phase.StartTime > TimeSpan.FromMinutes(expireUpcomingInMinutes))
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
        History = History.OrderBy(s => s.StartAt).ToList();
    } 
}
