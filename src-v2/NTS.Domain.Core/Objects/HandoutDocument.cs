using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public class HandoutDocument : DomainEntity
{
    private HandoutDocument()
    {
    }
    public HandoutDocument(
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
