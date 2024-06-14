﻿using static NTS.Domain.Enums.SnapshotType;
using static NTS.Domain.Core.Aggregates.Participations.SnapshotResultType;

namespace NTS.Domain.Core.Aggregates.Participations;

public class Phase : DomainEntity, IPhaseState
{
    internal double InternalGate { get; set; }
    private Timestamp? VetTime => ReinspectTime ?? InspectTime;
    private TimeSpan? LoopTime => ArriveTime - StartTime;
    private TimeSpan? PhaseTime => VetTime - StartTime;
    private bool IsFeiRulesAndNotFinal => CompetitionType == CompetitionType.FEI && !IsFinal;

    public Phase(double length, int maxRecovery, int rest, CompetitionType competitionType, bool isFinal, int? criRecovery)
    {
        Length = length;
        MaxRecovery = maxRecovery;
        Rest = rest;
        CompetitionType = competitionType;
        IsFinal = isFinal;
        CRIRecovery = criRecovery;
    }

    public string Gate => InternalGate.ToString("0.00");
    public double Length { get; private set; }
    public int MaxRecovery { get; private set; }
    public int Rest { get; private set; }
    public CompetitionType CompetitionType { get; private set; }
    public bool IsFinal { get; private set; }
    public int? CRIRecovery { get; private set; }
    public Timestamp? StartTime { get; internal set; }
    public Timestamp? ArriveTime { get; internal set; }
    public Timestamp? InspectTime { get; internal set; }
    public bool IsReinspectionRequested { get; internal set; }
    public Timestamp? ReinspectTime { get; internal set; }
    public bool IsRIRequested { get; internal set; }
    public bool IsCRIRequested { get; internal set; }
    public Timestamp? RequiredInspectionTime => VetTime?.Add(TimeSpan.FromMinutes(Rest - 15)); //TODO: settings?
    public Timestamp? OutTime => VetTime?.Add(TimeSpan.FromMinutes(Rest));
    public TimeSpan? Time => IsFeiRulesAndNotFinal ? PhaseTime : LoopTime;
    public TimeSpan? RecoverySpan => VetTime - ArriveTime;
    public double? AveregeLoopSpeed => Length / LoopTime?.TotalHours;
    public double? AveragePhaseSpeed => Length / PhaseTime?.TotalHours + RecoverySpan?.TotalHours;
    public double? AverageSpeed => IsFeiRulesAndNotFinal ? AveragePhaseSpeed : AveregeLoopSpeed;

    internal bool IsComplete => OutTime != null;

    internal SnapshotResult Arrive(Snapshot snapshot)
    {
        // TODO: settings - Add setting for separate final. This is useful for some events such as Shumen where we need separate detection for the actual final
        var isSeparateFinal = false;
        if (isSeparateFinal && snapshot.Type == Final && !IsFinal)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToSeparateStageLine);
        }
        if (isSeparateFinal && snapshot.Type == Stage && IsFinal)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToSeparateFinishLine);
        }
        if (ArriveTime != null)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToDuplicateArrive);
        }
        ArriveTime = snapshot.Timestamp;
        HandleCRI();

        return SnapshotResult.Applied(snapshot);
    }

    internal SnapshotResult Inspect(Snapshot snapshot)
    {
        if (IsReinspectionRequested && ReinspectTime != null || InspectTime != null)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToSeparateFinishLine);
        }
        if (IsReinspectionRequested)
        {
            ReinspectTime = snapshot.Timestamp;
        }
        else
        {
            InspectTime = snapshot.Timestamp;
        }
        HandleCRI();

        return SnapshotResult.Applied(snapshot);
    }

    internal void Update(IPhaseState state)
    {
        if (state.StartTime != null)
        {
            if (state.ArriveTime < state.StartTime)
            {
                throw new DomainException(nameof(ArriveTime), "Arrive Time cannot be sooner than Start Time");
            }
            if (state.InspectTime < state.StartTime)
            {
                throw new DomainException(nameof(InspectTime), "Inspect Time cannot be sooner than Start Time");
            }
            if (state.ReinspectTime < state.ArriveTime)
            {
                throw new DomainException(nameof(ReinspectTime), "Reinspect Time cannot be sooner than Start Time");
            }
        }
        StartTime = state.StartTime;
        ArriveTime = state.ArriveTime;
        InspectTime = state.InspectTime;
        ReinspectTime = state.ReinspectTime;
    }

    internal bool ViolatesRecoveryTime()
    {
        return RecoverySpan > TimeSpan.FromMinutes(MaxRecovery);
    }

    internal bool ViolatesSpeedRestriction(double? minSpeed, double? maxSpeed)
    {
        return AverageSpeed < minSpeed || AverageSpeed > maxSpeed;
    }

    private void HandleCRI()
    {
        if (CRIRecovery == null)
        {
            return;
        }
        IsCRIRequested = RecoverySpan >= TimeSpan.FromMinutes(CRIRecovery.Value);
    }
}

public interface IPhaseState
{
    int Id { get; }
    public Timestamp? StartTime { get; }
    public Timestamp? ArriveTime { get; }
    public Timestamp? InspectTime { get; }
    public Timestamp? ReinspectTime { get; }
}