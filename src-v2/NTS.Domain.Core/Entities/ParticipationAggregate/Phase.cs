namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Phase : DomainEntity
{
    private DateTimeOffset? VetTime => ReinspectionTime ?? InspectionTime;
    private TimeSpan? LoopTime => ArriveTime - StartTime;
    private TimeSpan? PhaseTime => VetTime - StartTime;

    public Phase(double length, int maxRecovery, int rest, CompetitionType competitionType, bool isFinal)
    {
        Length = length;
        MaxRecovery = maxRecovery;
        Rest = rest;
        CompetitionType = competitionType;
        IsFinal = isFinal;
    }

    public double Length { get; }
    public int MaxRecovery { get; }
    public int Rest { get; }
    public CompetitionType CompetitionType { get; }
    public bool IsFinal { get; }
    public DateTimeOffset? StartTime { get; internal set; }
    public DateTimeOffset? ArriveTime { get; internal set; }
    public DateTimeOffset? InspectionTime { get; internal set; }
    public bool IsReinspectionRequested { get; internal set; }
    public DateTimeOffset? ReinspectionTime { get; internal set; }
    public bool IsRIRequested { get; internal set; }
    public bool IsCRIRequested { get; internal set; }
    public DateTimeOffset? RequiredInspectionTime => VetTime?.AddMinutes(Rest - 15); //TODO: settings?
    public DateTimeOffset? OutTime => VetTime?.AddMinutes(Rest);
    public TimeSpan? Time => CompetitionType == CompetitionType.FEI && !IsFinal
        ? PhaseTime
        : LoopTime;
    public TimeSpan? RecoverySpan => VetTime - ArriveTime;
    public double? AveregeLoopSpeed => Length / LoopTime?.TotalHours;
    public double? AveragePhaseSpeed => Length / PhaseTime?.TotalHours + RecoverySpan?.TotalHours;

    internal bool IsComplete => OutTime != null;
}
