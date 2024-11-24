using Not.Domain.Base;

namespace NTS.Domain.Core.Objects.Documents.Base;

public abstract record Document : DomainObject
{
    public Document(DocumentHeader header)
    {
        Header = header;
    }

    public DocumentHeader Header { get; }
}
