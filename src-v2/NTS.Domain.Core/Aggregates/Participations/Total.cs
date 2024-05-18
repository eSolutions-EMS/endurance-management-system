namespace NTS.Domain.Core.Aggregates.Participations;

public record Total : DomainObject
{
    public Total(IEnumerable<Phase> completedPhases)
    {
        var totalLength = completedPhases.Sum(x => x.Length);
        RideSpan = completedPhases.Aggregate(TimeSpan.Zero, (result, x) => result + (x.ArriveTime!.Value - x.StartTime!.Value));
        RecoverySpan = completedPhases.Aggregate(TimeSpan.Zero, (result, x) => result + x.RecoverySpan!.Value);
        RecoverySpanWithoutFinal = (RecoverySpan - completedPhases.FirstOrDefault(x => x.IsFinal)?.RecoverySpan) ?? RecoverySpan;
        Span = RideSpan + RecoverySpan;
        AverageSpeed = totalLength / Span.TotalHours;
    }

    public double AverageSpeed { get; }
    public TimeSpan Span { get; }
    public TimeSpan RideSpan { get; }
    public TimeSpan RecoverySpan { get; }
    public TimeSpan RecoverySpanWithoutFinal { get; }
}
