using Newtonsoft.Json;
using NTS.Domain.Configuration;
using static NTS.Domain.Core.Aggregates.Participations.SnapshotResultType;

namespace NTS.Domain.Core.Aggregates.Participations;

public class Phase : DomainEntity, IPhaseState
{
    // TODO: settings - Add setting for separate final. This is useful for some events such as Shumen where we need separate detection for the actual final
    bool _isSeparateFinish = false;

    private Timestamp? VetTime => ReinspectTime ?? InspectTime;

    [JsonConstructor]
    private Phase(int id) : base(id) { }
    // TODO: remove after EMS imports are not longer necessary
    public Phase(
        double length,
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
        bool isCompulsoryRequiredInspectionRequested) : this(length, maxRecovery, rest, competitionType, isFinal, criRecovery)
    {
        StartTime = startTimestamp;
        if (arriveTime != null)
        {
            ArriveTime = new Timestamp(arriveTime.Value);
        }
        if (inspectTime != null)
        {
            InspectTime = new Timestamp(inspectTime.Value);
        }
        if (reinspectTime != null)
        {
            ReinspectTime = new Timestamp(reinspectTime.Value);
        }
        IsReinspectionRequested = isReinspectionRequested;
        IsRIRequested = isRequiredInspectionRequested;
        IsCRIRequested = isCompulsoryRequiredInspectionRequested;
    }
    public Phase(double length, int maxRecovery, int rest, CompetitionRuleset competitionType, bool isFinal, int? criRecovery)
    {
        Length = length;
        MaxRecovery = maxRecovery;
        Rest = rest;
        CompetitionRuleset = competitionType;
        IsFinal = isFinal;
        CRIRecovery = criRecovery;
    }

    public string Gate { get; private set; }
    public double Length { get; private set; }
    public int MaxRecovery { get; private set; }
    public int Rest { get; private set; }
    public CompetitionRuleset CompetitionRuleset { get; private set; }
    public bool IsFinal { get; private set; }
    public int? CRIRecovery { get; private set; } // TODO: int CRIRecovery? wtf?
    public Timestamp? StartTime { get; internal set; }
    public Timestamp? ArriveTime { get; private set; }
    public Timestamp? InspectTime { get; private set; } // TODO: domain consistency rename InspectTime -> PresentationTime (and others)
    public bool IsReinspectionRequested { get; internal set; }
    public Timestamp? ReinspectTime { get; private set; }
    public bool IsRIRequested { get; internal set; }
    public bool IsCRIRequested { get; internal set; }

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
        if (StaticOptions.ShouldOnlyUseAverageLoopSpeed(CompetitionRuleset))
        {
            return GetAverageLoopSpeed();
        }
        return IsFinal ? GetAverageLoopSpeed() : GetAveragePhaseSpeed();
    }

    public bool IsComplete()
    {
        if (IsReinspectionRequested && ReinspectTime == null)
        {
            return false;
        }
        if (ArriveTime == null || InspectTime == null)
        {
            return false;
        }
        return true;
    }

    internal SnapshotResult Process(Snapshot snapshot)
    {
        if (snapshot is FinishSnapshot finishSnapshot)
        {
            return Finish(finishSnapshot);
        }
        else if (snapshot is StageSnapshot stageSnapshot)
        {
            return Arrive(stageSnapshot);
        }
        else if (snapshot is VetgateSnapshot vetgateSnapshot)
        {
            return Inspect(vetgateSnapshot);
        }
        else
        {
            throw GuardHelper.Exception($"Invalid snapshot '{snapshot.GetType()}'");
        }
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
        return GetRecoverySpan() > TimeSpan.FromMinutes(MaxRecovery);
    }

    internal bool ViolatesSpeedRestriction(Speed? minSpeed, Speed? maxSpeed)
    {
        return GetAverageSpeed() < minSpeed || GetAverageSpeed() > maxSpeed;
    }

    internal void RequestRequiredInspection()
    {
        if (IsCRIRequested)
        {
            throw new DomainException("Required inspection is not valid, because there is already " +
                "a Compulsory required inspection for this participation");
        }

        IsRIRequested = true;
    }

    internal void DisableReinspection()
    {
        GuardHelper.ThrowIfDefault(IsReinspectionRequested);

        if (ReinspectTime != null)
        {
            throw new DomainException("Cannot disable Reinspection because time of Reinspection is already present");
        }

        IsReinspectionRequested = false;
    }

    internal void SetGate(int number, double totalDistanceSoFar)
    {
        Gate = $"GATE{number}/{totalDistanceSoFar:0.##}";
    }

    SnapshotResult Finish(FinishSnapshot snapshot)
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

    SnapshotResult Arrive(StageSnapshot snapshot)
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
        if (IsReinspectionRequested && ReinspectTime != null && InspectTime != null)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToDuplicateInspect);
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

    private void HandleCRI()
    {
        if (CRIRecovery == null)
        {
            return;
        }
        IsCRIRequested = GetRecoverySpan() >= TimeSpan.FromMinutes(CRIRecovery.Value);
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

public enum PhaseState
{
    Ongoing = 1,
    Arrived = 2,
    Presented = 3
}
