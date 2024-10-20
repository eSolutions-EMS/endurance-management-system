using Not.Localization;
using NTS.Domain.Core.Entities;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Aggregates.Participations;

public class PhaseCollection : ReadOnlyCollection<Phase>
{
    public PhaseCollection(IEnumerable<Phase> phases) : base(phases.ToList())
    {
        var distanceSoFar = 0d;
        foreach (var phase in phases)
        {
            distanceSoFar += phase.Length;
            var number = phases.NumberOf(phase);
            phase.SetGate(number, distanceSoFar);
        }
        Current = this.LastOrDefault(x => x.IsComplete()) ?? this.First();
    }

    public Phase Current { get; private set; }
    public double Distance => this.Sum(x => x.Length);
    
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
        if (isComplete && snapshot.Timestamp > Current.GetOutTime() + notProcessingWindow)
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
        var next = GetNext();
        next.StartTime = Current.GetOutTime();
    }

    internal bool SelectNext()
    {
        if (Current == this.Last())
        {
            return false;
        }
        Current = GetNext();
        return true;
    }

    private Phase GetNext()
    {
        var currentIndex = this.IndexOf(Current);
        return this[++currentIndex];
    }
}
