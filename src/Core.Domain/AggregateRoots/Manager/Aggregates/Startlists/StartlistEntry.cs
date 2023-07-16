using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.Participations;
using System;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartlistEntry : IComparable<StartlistEntry>, IEquatable<StartlistEntry>
{
    public StartlistEntry() { }

    internal StartlistEntry(Participation participation)
    {
        this.Number = participation.Participant.Number;
        this.Name = participation.Participant.Name;
        this.AthleteName = participation.Participant.Athlete.Name;
        this.CountryName = participation.Participant.Athlete.Country.Name;
        this.Distance = participation.Distance!.Value;
        this.StartTime = Performance.GetStartTime(participation);
        this.Stage = participation.Participant.LapRecords.Count;
    }
    
    public string Number { get; init; }
    public string Name { get; init; }
    public string AthleteName { get; init; }
    public string CountryName { get; init; }
    public double Distance { get; init; }
    public int Stage { get; init; }
    public DateTime StartTime { get; init; }
    public bool HasStarted => this.StartTime < DateTime.Now;

    public int CompareTo(StartlistEntry other)
    {
        var now = DateTime.Now;
        var thisDiff = this.StartTime - now;
        var otherDiff = other.StartTime - now;
        // Order past entries (HasStarted == true) by start time descending
        if (this.HasStarted && other.HasStarted)
        {
            if (thisDiff > otherDiff)
            {
                return -1;
            }
            return 1;
        }
        if (this.HasStarted && !other.HasStarted)
        {
            return 1;
        }
        if (!this.HasStarted && other.HasStarted)
        {
            return -1;
        }
        // Order by StartTime ascending
        if (thisDiff > otherDiff)
        {
            return 1;
        }
        return -1;
    }

    public bool Equals(StartlistEntry other)
    {
        return this.Number == other?.Number;
    }
}
