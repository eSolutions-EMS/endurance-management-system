using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Entities;

public class Handout : DomainEntity
{
    // TODO: Investigate why this cannot deserialize without private ctor, but Officials can
    // It's because Official's parameters are identical to it's properties while Hangout's arent
    // How to approach this consistently?
    private Handout(int id) : base(id)
    {
    }
    public Handout(Participation participation)
    {
        Competition = participation.Competition;
        Tandem = participation.Tandem;
        Phases = participation.Phases;
    }

    public string Competition { get; private set; } = default!;
    public Tandem Tandem { get; private set; } = default!;
    public PhaseCollection Phases { get; private set; } = default!;
}
