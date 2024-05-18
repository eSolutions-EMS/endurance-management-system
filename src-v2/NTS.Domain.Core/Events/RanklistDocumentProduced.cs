using Not.Events;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Events;

public record RanklistDocumentProduced : DomainObject, IEvent
{
    public RanklistDocumentProduced(DocumentHeader header, Ranklist ranklist)
    {
        Header = header;
        Ranklist = ranklist;
    }

    public DocumentHeader Header { get; private set; }
    public Ranklist Ranklist { get; private set; }
}
