using Not.Events;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Events;

public record HandoutDocumentProduced : DomainObject, IEvent
{
    public HandoutDocumentProduced(DocumentHeader header, Tandem tandem, PhaseCollection phases)
    {
        Header = header;
        Tandem = tandem;
        Phases = phases;
    }

    public DocumentHeader Header { get; }
    public Tandem Tandem { get; }
    public PhaseCollection Phases { get; }
}
