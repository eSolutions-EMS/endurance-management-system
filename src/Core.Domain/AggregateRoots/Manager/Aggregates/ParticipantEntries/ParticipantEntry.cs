using Core.Domain.State.Participations;
using System;
using System.Linq;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.ParticipantEntries;

public class ParticipantEntry : IComparable<ParticipantEntry>, IEquatable<ParticipantEntry>
{
    public ParticipantEntry() { }
    
    internal ParticipantEntry(Participation participation)
    {
        this.Number = participation.Participant.Number;
        this.Name = participation.Participant.Athlete.Name;
        this.LapNumber = participation.Participant.LapRecords.Count;
        var record = participation.Participant.LapRecords.Last();
        this.LapDistance = record.Lap.LengthInKm;
    }
    
    public int LapNumber { get; init; }
    public string Number { get; init; }
    public string Name { get; init; }
    public DateTime? ArriveTime { get; set; }
    public WitnessEventType Type { get; set; }
    public double LapDistance { get; set; }
    
    public int CompareTo(ParticipantEntry other)
    {
        var thisNumber = int.Parse(this.Number);
        var otherNumber = int.Parse(other.Number);
        if (thisNumber < otherNumber)
        {
            return -1;
        }
        return 1;
    }

    public override bool Equals(object obj)
    {
        if (obj is ParticipantEntry entry)
        {
            return this.Equals(entry);
        }
        return false;
    }

    public bool Equals(ParticipantEntry other)
    {
        return this.Number == other?.Number;
    }
}
