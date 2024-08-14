using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Entities;

public class Handout : DomainEntity
{
    public Handout(Participation participation)
    {
        Competition = participation.Competition;
        Tandem = participation.Tandem;
        Phases = participation.Phases;
    }

    public string Competition { get; private set; }
    public Tandem Tandem { get; private set; }
    public PhaseCollection Phases { get; private set; }
}
