using Not.Localization;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Aggregates.Participations;

public class PhaseCollection : ReadOnlyCollection<Phase>
{
    public PhaseCollection(IEnumerable<Phase> phases) : base(phases.ToList())
    {
        var gate = 0d;
        for (var i = 0; i < this.Count; i++)
        {
            var phase = this[i];
            var distanceFromStart = gate += phase.Length;
            phase.InternalGate = $"{i + 1}/{distanceFromStart:0.##}";
        }
        Current = this.LastOrDefault(x => x.IsComplete()) ?? this.First();
    }

    public Phase Current { get; private set; }
    internal int CurrentNumber => this.NumberOf(Current); // TODO: remove
    public double Distance => this.Sum(x => x.Length);
    internal Timestamp? OutTime => this.LastOrDefault(x => x.OutTime != null)?.OutTime; // TODO: remove
    
    public override string ToString()
    {
        var completed = this.Count(x => x.IsComplete());
        return $"{Distance}{"km".Localize()}: {completed}/{this.Count}";
    }

    internal SnapshotResult Process(Snapshot snapshot)
    {
        var isComplete = Current.IsComplete();
        if (isComplete && Current.IsFinal)
        {
            return SnapshotResult.NotApplied(snapshot, SnapshotResultType.NotAppliedDueToParticipationComplete);
        }
        var notProcessingWindow = TimeSpan.FromMinutes(30); // TODO settings: use settings?
        if (isComplete && snapshot.Timestamp > Current.OutTime + notProcessingWindow)
        {
            SelectNext();
        }
        return Current.Process(snapshot);
    }

    internal void StartNext()
    {
        if (!Current.IsComplete())
        {
            throw GuardHelper.Exception("Cannot start next phase while current is active");
        }
        var nextStartTime = Current.OutTime;
        var next = GetNext();
        next.StartTime = Current.OutTime;
    }

    private void SelectNext()
    {
        if (Current == this.Last())
        {
            throw new GuardException("Cannot select next Phase. Current Phase is the final phase");
        }
        Current = GetNext();
    }

    private Phase GetNext()
    {
        var currentIndex = this.IndexOf(Current);
        return this[++currentIndex];
    }
}
