using NTS.Domain.Core.Events;
using System.Collections.ObjectModel;
using static NTS.Domain.Enums.SnapshotType;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Participation : DomainEntity, IAggregateRoot
{
    private static readonly TimeSpan NOT_SNAPSHOTABLE_WINDOW = TimeSpan.FromMinutes(30);

    public Participation(Tandem tandem, IEnumerable<Phase> phases)
    {
        Tandem = tandem;
        Phases = new(phases.ToList());
    }

    public Tandem Tandem { get; private set; }
    public ReadOnlyCollection<Phase> Phases { get; private set; }
    public NotQualified? NotQualified { get; private set; }
    public Total? Total => Phases.Any(x => x.IsComplete)
        ? new Total(Phases.Where(x => x.IsComplete))
        : default;

    public void Snapshot(SnapshotType type, DateTimeOffset time)
    {
        GuardHelper.ThrowIfDefault(type);

        var phase = Phases.FirstOrDefault(x => !x.IsComplete);
        if (phase == null)
        {
            return;
        }
        if (time < phase.OutTime + NOT_SNAPSHOTABLE_WINDOW)
        {
            return;
        }
       
        if (type == Vet)
        {
            phase.Inspect(time);
        }
        else
        {
            phase.Arrive(type, time);
        }
        EvaluatePhase(phase);
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
        if (phase.IsComplete)
        {
            if (phase.RecoverySpan > TimeSpan.FromMinutes(phase.MaxRecovery))
            {
                NotQualified = new FailedToQualify(FTQCodes.OT);
            }
            if (phase.AverageSpeed < Tandem.MinAverageSpeedlimit || phase.AverageSpeed > Tandem.MaxAverageSpeedLimit)
            {
                NotQualified = new FailedToQualify(FTQCodes.SP);
            }
            PhaseCompletedEvent.Emit(Tandem.Number, Tandem.Name, Phases.NumberOf(phase), phase.Length, phase.OutTime, NotQualified != null);
        }
    }
}
