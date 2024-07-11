namespace NTS.Domain.Core.Aggregates.Participations;

public record Total : DomainObject
{
    public Total(IEnumerable<Phase> completedPhases)
    {
        var totalLength = completedPhases.Sum(x => x.Length);
        RideSpan = completedPhases.Aggregate(TimeSpan.Zero, (result, x) => result + (x.ArriveTime - x.StartTime));
        RecoverySpan = completedPhases.Aggregate(TimeSpan.Zero, (result, x) => result + x.RecoverySpan!.Value);
        RecoverySpanWithoutFinal = (RecoverySpan - completedPhases.FirstOrDefault(x => x.IsFinal)?.RecoverySpan) ?? RecoverySpan;
        Span = RideSpan + RecoverySpan;
        AverageSpeed = totalLength / Span.TotalHours;
    }

    public double AverageSpeed { get; private set; }
    public TimeSpan Span { get; private set; }
    public TimeSpan RideSpan { get; private set; }
    public TimeSpan RecoverySpan { get; private set; }
    public TimeSpan RecoverySpanWithoutFinal { get; private set; }
}
