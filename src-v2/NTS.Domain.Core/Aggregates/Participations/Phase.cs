using static NTS.Domain.Core.Aggregates.Participations.SnapshotResultType;

namespace NTS.Domain.Core.Aggregates.Participations;

public class Phase : DomainEntity, IPhaseState
{
    // TODO: settings - Add setting for separate final. This is useful for some events such as Shumen where we need separate detection for the actual final
    bool _isSeparateFinish = false;

    internal string InternalGate { get; set; }
    private Timestamp? VetTime => ReinspectTime ?? InspectTime;
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

    public string Gate => $"GATE{InternalGate}"; // TODO: fix InternalGate complexity
    public double Length { get; private set; }
    public int MaxRecovery { get; private set; }
    public int Rest { get; private set; }
    public CompetitionType CompetitionType { get; private set; }
    public bool IsFinal { get; private set; }
    public int? CRIRecovery { get; private set; } // TODO: int CRIRecovery? wtf?
    
    //> Temporarily set to public for EMS import testing
    public Timestamp? StartTime { get; set; }
    public Timestamp? ArriveTime { get; set; }
    public Timestamp? InspectTime { get; set; } // TODO: domain consistency rename InspectTime -> PresentationTime (and others)
    public bool IsReinspectionRequested { get; set; }

    public Timestamp? ReinspectTime { get; set; }
    public bool IsRIRequested { get; set; }
    public bool IsCRIRequested { get; set; }
    //< Temporarily set to public for EMS import testing

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
        return IsFeiRulesAndNotFinal ? GetAveragePhaseSpeed() : GetAverageLoopSpeed();
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

        IsRIRequested = false;
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
