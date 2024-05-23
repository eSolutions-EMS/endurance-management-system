using Not.Injection;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Services;

public class DocumentHandler : IDocumentHandler
{
    public Task Handle(Document document)
    {
        throw new NotImplementedException();
    }
}

public interface IDocumentHandler : ITransientService
{
    Task Handle(Document document);
}
