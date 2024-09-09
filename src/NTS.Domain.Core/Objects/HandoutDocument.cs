using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record HandoutDocument : DomainObject
{
    public HandoutDocument(Participation participation, Event enduranceEvent, IEnumerable<Official> officials)
    {
        Header = new DocumentHeader(participation.Competition, enduranceEvent.PopulatedPlace, enduranceEvent.EventSpan, officials);
        Tandem = participation.Tandem;
        Phases = participation.Phases;
    }

    public DocumentHeader Header { get; }
    public Tandem Tandem { get; }
    public PhaseCollection Phases { get; }
}
