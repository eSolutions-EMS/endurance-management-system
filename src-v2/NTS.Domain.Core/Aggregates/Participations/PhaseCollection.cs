using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Aggregates.Participations;

public class PhaseCollection : ReadOnlyCollection<Phase>
{
    public PhaseCollection(IEnumerable<Phase> phases) : base(phases.ToList())
    {
        var gate = 0d;
        foreach (var phase in this)
        {
            phase.InternalGate = gate += phase.Length;
        }
    }

    internal Phase? Current => this.FirstOrDefault(x => !x.IsComplete);
    internal int CurrentNumber => this.NumberOf(Current ?? this.Last());
    internal double Distance => this.Sum(x => x.Length);
    internal Timestamp? OutTime => this.LastOrDefault(x => x.OutTime != null)?.OutTime;
}
