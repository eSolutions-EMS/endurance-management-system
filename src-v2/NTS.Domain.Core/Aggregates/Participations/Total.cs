namespace NTS.Domain.Core.Aggregates.Participations;

public record Total : DomainObject
{
    public Total(IEnumerable<Phase> completedPhases)
    {
        var totalLength = completedPhases.Sum(x => x.Length);
        RideInterval = completedPhases.Aggregate(TimeInterval.Zero, (result, x) => (result + (x.ArriveTime - x.StartTime))!);
        RecoveryInterval = completedPhases.Aggregate(TimeInterval.Zero, (result, x) => (result + x.RecoverySpan)!);
        RecoveryIntervalWithoutFinal = (RecoveryInterval - completedPhases.FirstOrDefault(x => x.IsFinal)?.RecoverySpan) ?? RecoveryInterval;
        Interval = (RideInterval + RecoveryInterval)!;
        AverageSpeed = (totalLength / Interval)!.Value;
    }

    public double AverageSpeed { get; private set; }
    public TimeInterval Interval { get; private set; }
    public TimeInterval RideInterval { get; private set; }
    public TimeInterval RecoveryInterval { get; private set; }
    public TimeInterval RecoveryIntervalWithoutFinal { get; private set; }
}
