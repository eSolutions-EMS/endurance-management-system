using NTS.Domain.Core.Events;
using static NTS.Domain.Enums.SnapshotType;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Participation : DomainEntity, IAggregateRoot
{
    private static readonly TimeSpan NOT_SNAPSHOTABLE_WINDOW = TimeSpan.FromMinutes(30);
    private static readonly FailedToQualify OUT_OF_TIME = new FailedToQualify(FTQCodes.OT);
    private static readonly FailedToQualify SPEED_RESTRICTION = new FailedToQualify(FTQCodes.SP);
    public Participation(Tandem tandem, IEnumerable<Phase> phases)
    {
        Tandem = tandem;
        Phases = new(phases);
    }

    public Tandem Tandem { get; private set; }
    public PhaseCollection Phases { get; private set; }
    public NotQualified? NotQualified { get; private set; }
    public Total? Total => Phases.Any(x => x.IsComplete)
        ? new Total(Phases.Where(x => x.IsComplete))
        : default;

    public void Snapshot(SnapshotType type, DateTimeOffset time)
    {
        GuardHelper.ThrowIfDefault(type);

        if (NotQualified != null)
        {
            return;
        }
        if (Phases.Current == null)
        {
            return;
        }
        if (time < Phases.Current.OutTime + NOT_SNAPSHOTABLE_WINDOW)
        {
            return;
        }
       
        if (type == Vet)
        {
            Phases.Current.Inspect(time);
        }
        else
        {
            Phases.Current.Arrive(type, time);
        }
        EvaluatePhase(Phases.Current);
    }

    public void Edit(IPhaseState state)
    {
        var phase = Phases.FirstOrDefault(x => x.Id == state.Id);
        GuardHelper.ThrowIfNull(phase, $"Cannot edit phase - phase with ID '{state.Id}' does not exist");

        phase.Edit(state);
        EvaluatePhase(phase);
    }

    public void Withdraw()
    {
        NotQualified = new Withdrawn();
    }
    public void Retire()
    {
        NotQualified = new Retired();
    }

    public void Disqualify(string reason)
    {
        NotQualified = new Disqualified(reason);
    }

    public void FinishNotRanked(string reason)
    {
        NotQualified = new FinishedNotRanked(reason);
    }

    public void FailToQualify(FTQCodes code, string? reason)
    {
        if (reason == null)
        {
            NotQualified = new FailedToQualify(code);
        }
        else
        {
            NotQualified = new FailedToQualify(code, reason);
        }
    }

    private void EvaluatePhase(Phase phase)
    {
        if (phase.ViolatesRecoveryTime())
        {
            NotQualified = OUT_OF_TIME;
        }
        else if (phase.ViolatesSpeedRestriction(Tandem.MinAverageSpeedlimit, Tandem.MaxAverageSpeedLimit))
        {
            NotQualified = SPEED_RESTRICTION;
        }
        else if (NotQualified == OUT_OF_TIME || NotQualified == SPEED_RESTRICTION)
        {
            NotQualified = null;
        }

        if (phase.IsComplete)
        {
            PhaseCompletedEvent.Emit(Tandem.Number, Tandem.Name, Phases.NumberOf(phase), phase.Length, phase.OutTime, NotQualified != null);
        }
    }
}
