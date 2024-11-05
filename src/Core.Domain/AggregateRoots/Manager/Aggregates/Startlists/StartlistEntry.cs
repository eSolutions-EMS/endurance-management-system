using System;
using System.Linq;
using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.Participations;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartlistEntry : IComparable<StartlistEntry>, IEquatable<StartlistEntry>
{
    public StartlistEntry() { }

    internal StartlistEntry(Participation participation, int toSkip = 0)
    {
        this.Number = participation.Participant.Number;
        this.Name = participation.Participant.Name;
        this.AthleteName = participation.Participant.Athlete.Name;
        this.CountryName = participation.Participant.Athlete.Country.Name;
        this.Distance = participation.Distance!.Value;
        this.IsRestOver = this.StartTime < DateTime.Now;

        this.Stage = toSkip + 1;
        var lapRecords = participation.Participant.LapRecords.Skip(toSkip).ToList();
        var first = lapRecords.First();
        this.StartTime = first.StartTime;
    }

    internal StartlistEntry(Participation participation)
    {
        this.Number = participation.Participant.Number;
        this.Name = participation.Participant.Name;
        this.AthleteName = participation.Participant.Athlete.Name;
        this.CountryName = participation.Participant.Athlete.Country.Name;
        this.Distance = participation.Distance!.Value;
        this.IsRestOver = this.StartTime < DateTime.Now;
        this.Stage = participation.Participant.LapRecords.Count + 1;
        this.StartTime = Performance.GetLastNextStartTime(participation).Value;
    }

    public string Number { get; init; }
    public string Name { get; init; }
    public string AthleteName { get; init; }
    public string CountryName { get; init; }
    public double Distance { get; init; }
    public int Stage { get; init; }
    public DateTime StartTime { get; init; }
    public bool IsRestOver { get; internal set; }
    public bool IsLateStart { get; internal set; }

    public int CompareTo(StartlistEntry other)
    {
        var maximumLateStart = TimeSpan.FromMinutes(15);
        var now = DateTime.Now;
        var thisDiff = this.StartTime - now;
        var otherDiff = other.StartTime - now;
        var isRestOver = this.StartTime < now;
        var otherIsRestOver = other.StartTime < now;
        var isLateStart = this.StartTime < now && this.StartTime > now - maximumLateStart;
        var otherIsLateStart = other.StartTime < now && other.StartTime > now - maximumLateStart;

        // Proprety assignment in order to render them differently in UI, stupid I know
        if (isRestOver != this.IsRestOver)
        {
            this.IsRestOver = isRestOver;
        }
        if (otherIsRestOver != other.IsRestOver)
        {
            other.IsRestOver = otherIsRestOver;
        }
        if (this.IsLateStart != isLateStart)
        {
            this.IsLateStart = isLateStart;
        }
        if (other.IsLateStart != otherIsLateStart)
        {
            other.IsLateStart = otherIsLateStart;
        }

        if (this.IsRestOver && !other.IsRestOver)
        {
            return 1;
        }
        if (!this.IsRestOver && other.IsRestOver)
        {
            return -1;
        }
        // Order past entries (IsRestOver == true) by start time descending
        if (isRestOver && otherIsRestOver)
        {
            if (thisDiff > otherDiff)
            {
                return -1;
            }
            else if (thisDiff < otherDiff)
            {
                return 1;
            }
            return int.Parse(this.Number) - int.Parse(other.Number);
        }
        // Order by StartTime ascending
        if (thisDiff > otherDiff)
        {
            return 1;
        }
        else if (thisDiff < otherDiff)
        {
            return -1;
        }
        return 0;
    }

    public bool Equals(StartlistEntry other)
    {
        return this.Number == other?.Number;
    }
}
