using static NTS.Domain.Enums.SnapshotType;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Phase : DomainEntity, IPhaseState
{
    private double _gate;
    private DateTimeOffset? VetTime => ReinspectTime ?? InspectTime;
    private TimeSpan? LoopTime => ArriveTime - StartTime;
    private TimeSpan? PhaseTime => VetTime - StartTime;
    private bool IsFeiRulesAndNotFinal => CompetitionType == CompetitionType.FEI && !IsFinal;

    public Phase(double gate, double length, int maxRecovery, int rest, CompetitionType competitionType, bool isFinal)
    {
        _gate = gate;
        Length = length;
        MaxRecovery = maxRecovery;
        Rest = rest;
        CompetitionType = competitionType;
        IsFinal = isFinal;
    }

    public string Gate => _gate.ToString("0.00");
    public double Length { get; }
    public int MaxRecovery { get; }
    public int Rest { get; }
    public CompetitionType CompetitionType { get; }
    public bool IsFinal { get; }
    public DateTimeOffset? StartTime { get; internal set; }
    public DateTimeOffset? ArriveTime { get; internal set; }
    public DateTimeOffset? InspectTime { get; internal set; }
    public bool IsReinspectionRequested { get; internal set; }
    public DateTimeOffset? ReinspectTime { get; internal set; }
    public bool IsRIRequested { get; internal set; }
    public bool IsCRIRequested { get; internal set; }
    public DateTimeOffset? RequiredInspectionTime => VetTime?.AddMinutes(Rest - 15); //TODO: settings?
    public DateTimeOffset? OutTime => VetTime?.AddMinutes(Rest);
    public TimeSpan? Time => IsFeiRulesAndNotFinal ? PhaseTime : LoopTime;
    public TimeSpan? RecoverySpan => VetTime - ArriveTime;
    public double? AveregeLoopSpeed => Length / LoopTime?.TotalHours;
    public double? AveragePhaseSpeed => Length / PhaseTime?.TotalHours + RecoverySpan?.TotalHours;
    public double? AverageSpeed => IsFeiRulesAndNotFinal ? AveragePhaseSpeed : AveregeLoopSpeed;

    internal bool IsComplete => OutTime != null;

    internal void Arrive(SnapshotType type, DateTimeOffset time)
    {
        // TODO: settings - Add setting for separate final. This is useful for some events such as Shumen where we need separate detection for the actual final
        var isSeparateFinal = false;
        if (isSeparateFinal && type == Final && !IsFinal)
        {
            return; //TODO: consider creating a specific SwallowException that is intended to interrupt the execution silently
        }
        if (isSeparateFinal && type == Stage && IsFinal)
        {
            return;
        }
        if (ArriveTime != null)
        {
            return;
        }
        ArriveTime = time;
    }

    internal void Inspect(DateTimeOffset time)
    {
        if (IsReinspectionRequested && ReinspectTime != null || InspectTime != null)
        {
            return;
        }
        if (IsReinspectionRequested)
        {
            ReinspectTime = time;
        }
        else
        {
            InspectTime = time;
        }
    }

    internal void Edit(IPhaseState state)
    {
        if (state.StartTime.HasValue)
        {
            if (state.ArriveTime < state.StartTime)
            {
                throw new DomainException(nameof(Phase.ArriveTime), "Arrive Time cannot be sooner than Start Time");
            }
            if (state.InspectTime < state.StartTime)
            {
                throw new DomainException(nameof(Phase.InspectTime), "Inspect Time cannot be sooner than Start Time");
            }
            if (state.ReinspectTime < state.ArriveTime)
            {
                throw new DomainException(nameof(Phase.ReinspectTime), "Reinspect Time cannot be sooner than Start Time");
            }
        }
        StartTime = state.StartTime;
        ArriveTime = state.ArriveTime;
        InspectTime = state.InspectTime;
        ReinspectTime = state.ReinspectTime;
    }
}

public interface IPhaseState
{
    int Id { get; }
    public DateTimeOffset? StartTime { get; }
    public DateTimeOffset? ArriveTime { get; }
    public DateTimeOffset? InspectTime { get; }
    public DateTimeOffset? ReinspectTime { get; }
}
