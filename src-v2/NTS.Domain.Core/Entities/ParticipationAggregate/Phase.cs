﻿using Newtonsoft.Json;
using NTS.Domain.Core.Configuration;
using static NTS.Domain.Core.Entities.SnapshotResultType;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Phase : DomainEntity, IPhaseState
{
    public static Phase ImportFromEMS(double length,
        int maxRecovery,
        int rest,
        CompetitionRuleset competitionType,
        bool isFinal,
        int? criRecovery,
        Timestamp startTimestamp,
        DateTime? arriveTime,
        DateTime? inspectTime,
        DateTime? reinspectTime,
        bool isReinspectionRequested,
        bool isRequiredInspectionRequested,
        bool isCompulsoryRequiredInspectionRequested)
    {
        var phase = new Phase(length, maxRecovery, rest, competitionType, isFinal, criRecovery);

        phase.StartTime = startTimestamp;
        if (arriveTime != null)
        {
            phase.ArriveTime = new Timestamp(arriveTime.Value);
        }
        if (inspectTime != null)
        {
            phase.PresentTime = new Timestamp(inspectTime.Value);
        }
        if (reinspectTime != null)
        {
            phase.RepresentTime = new Timestamp(reinspectTime.Value);
        }
        phase.IsReinspectionRequested = isReinspectionRequested;
        phase.IsRequiredInspectionRequested = isRequiredInspectionRequested;
        phase.IsRequiredInspectionCompulsory = isCompulsoryRequiredInspectionRequested;

        return phase;
    }

    // TODO: settings - Add setting for separate final. This is useful for some events such as Shumen where we need separate detection for the actual final
    bool _isSeparateFinish = false;

    private Timestamp? VetTime => RepresentTime ?? PresentTime;

    [JsonConstructor]
    private Phase(
        int id,
        string gate,
        double length,
        int maxRecovery,
        int rest,
        CompetitionRuleset ruleset,
        bool isFinal,
        int? requiredImspectionCompulsoryThreshold,
        Timestamp? startTime,
        Timestamp? arriveTime,
        Timestamp? presentTime,
        Timestamp? representTime,
        bool isRepresentationRequested,
        bool isRequiredInspectionRequested,
        bool isRequiredInspectionCompulsory) : base(id)
    {
        Gate = gate;
        Length = length;
        MaxRecovery = maxRecovery;
        Rest = rest;
        Ruleset = ruleset;
        IsFinal = isFinal;
        StartTime = startTime;
        ArriveTime = arriveTime;
        PresentTime = presentTime;
        RepresentTime = representTime;
        IsReinspectionRequested = isRepresentationRequested;
        IsRequiredInspectionRequested = isRequiredInspectionRequested;
        IsRequiredInspectionCompulsory = isRequiredInspectionCompulsory;
        RequiredInspectionCompulsoryThreshold = requiredImspectionCompulsoryThreshold;
    }

    public Phase(
        double length,
        int maxRecovery,
        int rest,
        CompetitionRuleset competitionRuleset,
        bool isFinal,
        int? compulsoryRequiredInspectionTreshold)
        : this(
              GenerateId(),
              "",
              length,
              maxRecovery,
              rest,
              competitionRuleset,
              isFinal,
              compulsoryRequiredInspectionTreshold,
              null,
              null,
              null,
              null,
              false,
              false,
              false)
    {
    }

    public string Gate { get; private set; }
    public double Length { get; private set; }
    public int MaxRecovery { get; private set; }
    public int Rest { get; private set; }
    public CompetitionRuleset Ruleset { get; private set; }
    public bool IsFinal { get; private set; }
    public Timestamp? StartTime { get; internal set; }
    public Timestamp? ArriveTime { get; private set; }
    public Timestamp? PresentTime { get; private set; }
    public Timestamp? RepresentTime { get; private set; }
    public bool IsReinspectionRequested { get; internal set; }
    public bool IsRequiredInspectionRequested { get; internal set; }
    public bool IsRequiredInspectionCompulsory { get; internal set; }
    public int? RequiredInspectionCompulsoryThreshold { get; private set; }

    public Timestamp? GetRequiredInspectionTime()
    {
        return VetTime?.Add(TimeSpan.FromMinutes(Rest - 15)); //TODO: settings
    }

    public Timestamp? GetOutTime()
    {
        if (ArriveTime == null)
        {
            return null;
        }
        return VetTime?.Add(TimeSpan.FromMinutes(Rest));
    }

    public TimeInterval? GetLoopSpan()
    {
        return ArriveTime - StartTime;
    }

    public TimeInterval? GetPhaseSpan()
    {
        return VetTime - StartTime;
    }

    public TimeInterval? GetRecoverySpan()
    {
        return VetTime - ArriveTime;
    }

    public Speed? GetAverageLoopSpeed()
    {
        return Length / GetLoopSpan();
    }

    public Speed? GetAveragePhaseSpeed()
    {
        return Length / GetPhaseSpan();
    }

    public Speed? GetAverageSpeed()
    {
        if (StaticOptions.ShouldOnlyUseAverageLoopSpeed(Ruleset))
        {
            return GetAverageLoopSpeed();
        }
        return IsFinal ? GetAverageLoopSpeed() : GetAveragePhaseSpeed();
    }

    public bool IsComplete()
    {
        if (IsReinspectionRequested && RepresentTime == null)
        {
            return false;
        }
        if (ArriveTime == null || PresentTime == null)
        {
            return false;
        }
        return true;
    }

    internal SnapshotResult Process(Snapshot snapshot)
    {
        return snapshot.Type switch
        {
            SnapshotType.Vet => Inspect(snapshot),
            SnapshotType.Stage => Arrive(snapshot),
            SnapshotType.Final => Finish(snapshot),
            SnapshotType.Automatic => Automatic(snapshot),
            _ => throw GuardHelper.Exception($"Invalid snapshot '{snapshot.GetType()}'"),
        };
    }

    internal void Update(IPhaseState state)
    {
        if (state.StartTime != null)
        {
            if (state.ArriveTime < state.StartTime)
            {
                throw new DomainException(nameof(ArriveTime), "Arrive Time cannot be sooner than Start Time");
            }
            if (state.PresentTime < state.StartTime)
            {
                throw new DomainException(nameof(PresentTime), "Inspect Time cannot be sooner than Start Time");
            }
            if (state.RepresentTime < state.ArriveTime)
            {
                throw new DomainException(nameof(RepresentTime), "Reinspect Time cannot be sooner than Start Time");
            }
        }
        StartTime = state.StartTime;
        ArriveTime = state.ArriveTime;
        PresentTime = state.PresentTime;
        RepresentTime = state.RepresentTime;
    }

    internal bool ViolatesRecoveryTime()
    {
        return GetRecoverySpan() > TimeSpan.FromMinutes(MaxRecovery);
    }

    internal bool ViolatesSpeedRestriction(Speed? minSpeed, Speed? maxSpeed)
    {
        return GetAverageSpeed() < minSpeed || GetAverageSpeed() > maxSpeed;
    }

    internal void RequestRequiredInspection()
    {
        if (IsRequiredInspectionRequested)
        {
            return;
        }
        if (IsRequiredInspectionCompulsory)
        {
            throw new DomainException("Required inspection is compulsory");
        }
        IsRequiredInspectionRequested = true;
    }

    internal void DisableReinspection()
    {
        if (!IsReinspectionRequested)
        {
            return;
        }
        if (RepresentTime != null)
        {
            throw new DomainException("Cannot disable Reinspection because time of Reinspection is already present");
        }
        IsReinspectionRequested = false;
    }

    internal void SetGate(int number, double totalDistanceSoFar)
    {
        Gate = $"GATE{number}/{totalDistanceSoFar:0.##}";
    }

    SnapshotResult Automatic(Snapshot snapshot)
    {
        if (ArriveTime == null && IsFinal)
        {
            return Finish(snapshot);
        }
        if (ArriveTime == null)
        {
            return Arrive(snapshot);
        }
        if (VetTime == null)
        {
            return Inspect(snapshot);
        }
        return SnapshotResult.NotApplied(snapshot, NotAppliedDueToInapplicableAutomatic);
    }

    SnapshotResult Finish(Snapshot snapshot)
    {
        if (_isSeparateFinish && !IsFinal)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToSeparateStageLine);
        }
        if (ArriveTime != null)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToDuplicateArrive);
        }

        ArriveTime = snapshot.Timestamp;
        HandleCRI();
        return SnapshotResult.Applied(snapshot);
    }

    SnapshotResult Arrive(Snapshot snapshot)
    {
        if (_isSeparateFinish && IsFinal)
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

    SnapshotResult Inspect(Snapshot snapshot)
    {
        if (IsReinspectionRequested && RepresentTime != null && PresentTime != null)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToDuplicateInspect);
        }

        if (IsReinspectionRequested)
        {
            RepresentTime = snapshot.Timestamp;
        }
        else
        {
            PresentTime = snapshot.Timestamp;
        }
        HandleCRI();
        return SnapshotResult.Applied(snapshot);
    }

    private void HandleCRI()
    {
        if (RequiredInspectionCompulsoryThreshold == null)
        {
            return;
        }
        IsRequiredInspectionCompulsory = GetRecoverySpan() >= TimeSpan.FromMinutes(RequiredInspectionCompulsoryThreshold.Value);
    }
}

public interface IPhaseState
{
    int Id { get; }
    public Timestamp? StartTime { get; }
    public Timestamp? ArriveTime { get; }
    public Timestamp? PresentTime { get; }
    public Timestamp? RepresentTime { get; }
}