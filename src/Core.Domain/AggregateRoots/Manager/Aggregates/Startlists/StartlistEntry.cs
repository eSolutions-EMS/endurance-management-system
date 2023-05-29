using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.Participations;
using System;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartlistEntry : IComparable<StartlistEntry>
{
    public StartlistEntry() { }

    internal StartlistEntry(Participation participation)
    {
        Number = participation.Participant.Number;
        Name = participation.Participant.Name;
        AthleteName = participation.Participant.Athlete.Name;
        CountryName = participation.Participant.Athlete.Country.Name;
        Distance = participation.Distance!.Value;
        StartTime = Performance.GetStartTime(participation);
    }
    
    public string Number { get; init; }
    public string Name { get; init; }
    public string AthleteName { get; init; }
    public string CountryName { get; init; }
    public double Distance { get; init; }
    public DateTime StartTime { get; init; }
    public bool HasStarted { get; init; }

    public int CompareTo(StartlistEntry other)
    {
        var now = DateTime.Now;
        var thisDiff = this.StartTime - now;
        var otherDiff = other.StartTime - now;
        // Order entries with passed time separately
        if (thisDiff.TotalSeconds < 0 && otherDiff.TotalSeconds < 0)
        {
            if (thisDiff > otherDiff)
            {
                return 1;
            }
            return -1;
        }
        // Order by StartTime ascending
        if (thisDiff > otherDiff)
        {
            return -1;
        }
        return 1;
    }
}
