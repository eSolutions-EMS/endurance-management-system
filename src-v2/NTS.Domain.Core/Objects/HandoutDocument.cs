using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record HandoutDocument : Document
{
    public HandoutDocument(
        Participation participation,
        Event @event,
        IEnumerable<Official> officials)
        : base(new DocumentHeader(participation.Competition, @event.PopulatedPlace, @event.EventSpan, officials))
    {
        Tandem = participation.Tandem;
        Phases = participation.Phases;
    }

    public Tandem Tandem { get; private set; }
    public PhaseCollection Phases { get; private set; }
}
