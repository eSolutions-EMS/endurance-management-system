using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record HandoutDocument : DomainObject
{
    public HandoutDocument(Handout handout, Event @event, IEnumerable<Official> officials)
    {
        Header = new DocumentHeader(handout.Competition, @event.PopulatedPlace, @event.EventSpan, officials);
        Tandem = handout.Tandem;
        Phases = handout.Phases;
    }

    public DocumentHeader Header { get; }
    public Tandem Tandem { get; }
    public PhaseCollection Phases { get; }
}
