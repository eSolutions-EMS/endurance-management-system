using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.Participations;
using System;
using System.Linq;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;

public class ArrivelistEntry : IComparable<ArrivelistEntry>
{
    public ArrivelistEntry() { }
    
    internal ArrivelistEntry(Participation participation)
    {
        this.Number = participation.Participant.Number;
        this.Name = participation.Participant.Athlete.Name;

        var record = participation.Participant.LapRecords.Last();
        var averageSpeed = Performance
            .GetAll(participation)
            .Where(x => x.AverageSpeed.HasValue)
            .Aggregate(0d, (total, next) => total += next.AverageSpeed.Value);
        this.StartTime = record.StartTime;
        this.AverageSpeed = averageSpeed;
        this.LapDistance = record.Lap.LengthInKm;
    }
    
    public string Number { get; init; }
    public string Name { get; init; }
    public DateTime? ArriveTime { get; set; }
    public WitnessEventType Type { get; set; }
    public DateTime StartTime { get; set; }
    public double AverageSpeed { get; set; }
    public double LapDistance { get; set; }
    
    public int CompareTo(ArrivelistEntry other)
    {
        var thisEstimatedHours = this.LapDistance / this.AverageSpeed;
        var otherExpectedHours = other.LapDistance / other.AverageSpeed;

        var thisEstimatedArr = this.StartTime + TimeSpan.FromHours(thisEstimatedHours);
        var otherEstimatedArr = other.StartTime + TimeSpan.FromHours(otherExpectedHours);
        if (thisEstimatedArr < otherEstimatedArr)
        {
            return -1;
        }
        return 1;
    }
}
