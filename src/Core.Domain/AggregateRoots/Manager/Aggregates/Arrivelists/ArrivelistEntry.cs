using Core.Domain.State.Participations;
using System;
using System.Linq;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;

public class ArrivelistEntry : IComparable<ArrivelistEntry>, IEquatable<ArrivelistEntry>
{
    public ArrivelistEntry() { }
    
    internal ArrivelistEntry(Participation participation)
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
    
    public int CompareTo(ArrivelistEntry other)
    {
        var thisNumber = int.Parse(this.Number);
        var otherNumber = int.Parse(other.Number);
        if (thisNumber < otherNumber)
        {
            return -1;
        }
        return 1;
    }

    public bool Equals(ArrivelistEntry other)
    {
        return this.Number == other?.Number;
    }
}
