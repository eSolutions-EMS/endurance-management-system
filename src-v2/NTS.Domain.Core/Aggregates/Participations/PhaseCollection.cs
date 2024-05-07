using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

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
}
