using Not.Events;
using NTS.Domain.Core.Events;
using NTS.Domain.Core.Objects;
using static NTS.Domain.Enums.SnapshotType;

namespace NTS.Domain.Core.Aggregates.Participations;

public class Participation : DomainEntity, IAggregateRoot
{
    private static readonly TimeSpan NOT_SNAPSHOTABLE_WINDOW = TimeSpan.FromMinutes(30);
    private static readonly FailedToQualify OUT_OF_TIME = new FailedToQualify(FTQCodes.OT);
    private static readonly FailedToQualify SPEED_RESTRICTION = new FailedToQualify(FTQCodes.SP);
    public Participation(string competition, Tandem tandem, IEnumerable<Phase> phases)
    {
        Competition = competition;
        Tandem = tandem;
        Phases = new(phases);
    }

    public string Competition { get; }
    public Tandem Tandem { get; private set; }
    public PhaseCollection Phases { get; private set; }
    public NotQualified? NotQualified { get; private set; }
    public bool IsNotQualified => NotQualified != null;
    public Total? Total => Phases.Any(x => x.IsComplete)
        ? new Total(Phases.Where(x => x.IsComplete))
        : default;

    public void Process(Snapshot snapshot)
    {
        if (NotQualified != null)
        {
            return;
        }
        if (Phases.Current == null)
        {
            return;
        }
        if (snapshot.Timestamp < Phases.Current.OutTime + NOT_SNAPSHOTABLE_WINDOW)
        {
            return;
        }

        if (snapshot.Type == Vet)
        {
            Phases.Current.Inspect(snapshot);
        }
        else
        {
            Phases.Current.Arrive(snapshot);
        }
        EvaluatePhase(Phases.Current);
    }

    public void Update(IPhaseState state)
    {
        var phase = Phases.FirstOrDefault(x => x.Id == state.Id);
        GuardHelper.ThrowIfDefault(phase);

        phase.Update(state);
        EvaluatePhase(phase);
    }

    public void Withdraw()
    {
        RevokeQualification(new Withdrawn());
    }
    public void Retire()
    {
        RevokeQualification(new Retired());
    }

    public void Disqualify(string reason)
    {
        RevokeQualification(new Disqualified(reason));
    }

    public void FinishNotRanked(string reason)
    {
        RevokeQualification(new FinishedNotRanked(reason));
    }

    public void FailToQualify(FTQCodes code)
    {
        RevokeQualification(new FailedToQualify(code));
    }

    public void FailToCompleteLoop(string reason)
    {
        RevokeQualification(new FailedToQualify(reason));
    }

    public void RestoreQualification()
    {
        NotQualified = null;
        var qualificationRestored = new QualificationRestored(Tandem.Number);
        EventHelper.Emit(qualificationRestored);
    }

    private void EvaluatePhase(Phase phase)
    {
        if (phase.ViolatesRecoveryTime())
        {
            RevokeQualification(OUT_OF_TIME);
            return;
        }
        if (phase.ViolatesSpeedRestriction(Tandem.MinAverageSpeedlimit, Tandem.MaxAverageSpeedLimit))
        {
            RevokeQualification(SPEED_RESTRICTION);
            return;
        }
        if (NotQualified == OUT_OF_TIME || NotQualified == SPEED_RESTRICTION)
        {
            RestoreQualification();
        }
        if (phase.IsComplete)
        {
            var phaseCompleted = new PhaseCompleted(Tandem.Number, Tandem.Name, Phases.NumberOf(phase), phase.Length, phase.OutTime);
            EventHelper.Emit(phaseCompleted);
        }
    }

    private void RevokeQualification(NotQualified notQualified)
    {
        NotQualified = notQualified;
        var qualificationRevoked = new QualificationRevoked(Tandem.Number, notQualified);
        EventHelper.Emit(qualificationRevoked);
    }
}
