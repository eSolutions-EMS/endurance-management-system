using Newtonsoft.Json;
using Not.Domain.Base;
using Not.Events;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Objects.Payloads;
using static NTS.Domain.Core.Aggregates.SnapshotResultType;

namespace NTS.Domain.Core.Aggregates;

public class Participation : AggregateRoot, IAggregateRoot
{
    //static readonly TimeSpan NOT_SNAPSHOTABLE_WINDOW = TimeSpan.FromMinutes(30);
    static readonly FailedToQualify OUT_OF_TIME = new([FtqCode.OT]);
    static readonly FailedToQualify SPEED_RESTRICTION = new([FtqCode.SP]);
    public static readonly Event<PhaseCompleted> PHASE_COMPLETED_EVENT = new();
    public static readonly Event<ParticipationEliminated> ELIMINATED_EVENT = new();
    public static readonly Event<ParticipationRestored> RESTORED_EVENT = new();

    [JsonConstructor]
    Participation(
        int id,
        Competition competition,
        Combination combination,
        PhaseCollection phases,
        Eliminated? notQualified
    )
        : base(id)
    {
        Competition = competition;
        Combination = combination;
        Phases = phases;
        Eliminated = notQualified;
    }

    public Participation(
        string competitionName,
        CompetitionRuleset ruleset,
        CompetitionType type,
        Combination combination,
        IEnumerable<Phase> phases
    )
        : this(GenerateId(), new(competitionName, ruleset, type), combination, new(phases), null)
    { }

    public Competition Competition { get; }
    public Combination Combination { get; }
    public PhaseCollection Phases { get; }
    public Eliminated? Eliminated { get; private set; }

    public bool IsEliminated()
    {
        return Eliminated != null;
    }

    public bool IsComplete()
    {
        return Phases.All(x => x.IsComplete());
    }

    public Total? GetTotal()
    {
        if (Phases.All(x => !x.IsComplete()))
        {
            return null;
        }
        return new Total(Phases);
    }

    public override string ToString()
    {
        return Combine(Combination, Phases, Eliminated);
    }

    //TODO rename to smthing better (including ISnapshotProcessor, IManualProcessor and other mentions..)
    public SnapshotResult Process(Snapshot snapshot, Action<string> log)
    {
        if (Eliminated != null)
        {
            return SnapshotResult.NotApplied(snapshot, NotAppliedDueToNotQualified);
        }

        log("-------- RPC -------- Processing..");
        var result = Phases.Process(snapshot);
        EvaluatePhase(Phases.Current, log);

        return result;
    }

    public void Update(IPhaseState state)
    {
        var phase = Phases.FirstOrDefault(x => x.Id == state.Id);
        GuardHelper.ThrowIfDefault(phase);

        phase.Update(state);
        EvaluatePhase(phase);
    }

    public void ChangeReinspection(bool isRequested)
    {
        if (isRequested)
        {
            Phases.Current.IsReinspectionRequested = true;
        }
        else
        {
            Phases.Current.DisableReinspection();
        }
    }

    public void ChangeRequiredInspection(bool isRequested)
    {
        if (isRequested)
        {
            Phases.Current.RequestRequiredInspection();
        }
        else
        {
            Phases.Current.IsRequiredInspectionRequested = false;
        }
    }

    public void Withdraw()
    {
        Eliminate(new Withdrawn());
    }

    public void Retire()
    {
        Eliminate(new Retired());
    }

    public void Disqualify(string reason)
    {
        Eliminate(new Disqualified(reason));
    }

    public void FinishNotRanked(string reason)
    {
        Eliminate(new FinishedNotRanked(reason));
    }

    public void FailToQualify(FtqCode[] codes, string? reason)
    {
        Eliminate(new FailedToQualify(codes, reason));
    }

    public void Restore()
    {
        Eliminated = null;
        var qualificationRestored = new ParticipationRestored(this);
        RESTORED_EVENT.Emit(qualificationRestored);
    }

    void EvaluatePhase(Phase phase, Action<string>? log = null)
    {
        log?.Invoke("-------- RPC -------- EvaluatingPhase..");
        if (phase.ViolatesRecoveryTime())
        {
            log?.Invoke("-------- RPC -------- Eliminating (recovery)..");
            Eliminate(OUT_OF_TIME);
            return;
        }
        var violates = phase.ViolatesSpeedRestriction(
            Combination.MinAverageSpeed,
            Combination.MaxAverageSpeed
        );
        log?.Invoke($"-------- RPC --------- Min average speed: {Combination.MinAverageSpeed}");
        log?.Invoke($"-------- RPC --------- Max average speed: {Combination.MaxAverageSpeed}");
        var loopMessage =
            $"-------- RPC --------- Phase average loop speed: {phase.GetAverageLoopSpeed()}";
        log?.Invoke(loopMessage);
        var phaseMessage =
            $"-------- RPC --------- Phase average phase speed: {phase.GetAveragePhaseSpeed()}";
        log?.Invoke(phaseMessage);
        log?.Invoke($"----------- RPC ---------- Is final: {phase.IsFinal}");
        log?.Invoke($"-------- RPC --------- Violates: {violates}");

        log?.Invoke($"-------- RPC --------- Length: {phase.Length}");
        log?.Invoke($"-------- RPC --------- StartTime: {phase.StartTime}");
        log?.Invoke($"-------- RPC --------- ArriveTime: {phase.ArriveTime}");
        var obj = $"-------- RPC --------- LoopSpan: {phase.GetLoopSpan()}";
        log?.Invoke(obj);

        if (violates)
        {
            log?.Invoke("-------- RPC -------- Eliminating (speed)..");
            Eliminate(SPEED_RESTRICTION, log);
            return;
        }
        if (Eliminated == OUT_OF_TIME || Eliminated == SPEED_RESTRICTION)
        {
            log?.Invoke("-------- RPC -------- Restoring..");
            Restore();
        }
        if (phase.IsComplete())
        {
            log?.Invoke("-------- RPC -------- Phase is completed..");
            Phases.StartIfNext();
            var phaseCompleted = new PhaseCompleted(this);
            PHASE_COMPLETED_EVENT.Emit(phaseCompleted);
        }
        log?.Invoke("-------- RPC -------- End..");
    }

    void Eliminate(Eliminated notQualified, Action<string>? log = null)
    {
        log?.Invoke("-------- RPC --------- Eliminate 2");
        Eliminated = notQualified;
        log?.Invoke("-------- RPC --------- Eliminate 3");
        var qualificationRevoked = new ParticipationEliminated(this);
        log?.Invoke("-------- RPC --------- Eliminate 4");
        ELIMINATED_EVENT.Emit(qualificationRevoked);
        log?.Invoke("-------- RPC --------- Eliminated invoked");
    }
}
