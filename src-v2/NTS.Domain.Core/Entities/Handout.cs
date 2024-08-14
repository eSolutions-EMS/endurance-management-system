using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Entities;

public class Handout : DomainEntity
{
    private Handout()
    {
    }
    public Handout(
        Participation participation,
        Event @event,
        IEnumerable<Official> officials)
        : base()
    {
        // TODO: Should we keep this as part of the entity?
        Header = new DocumentHeader(participation.Competition, @event.PopulatedPlace, @event.EventSpan, officials);
        Tandem = participation.Tandem;
        Phases = participation.Phases;
    }

    public DocumentHeader Header { get; private set; }
    public Tandem Tandem { get; private set; }
    public PhaseCollection Phases { get; private set; }
}
