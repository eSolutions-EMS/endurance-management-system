using Not.Injection;

namespace NTS.Judge.Blazor.Ports;

public interface IDocumentBehind : ITransientService // TODO: remove
{
    Task CreateRanklist(int classificationId);
}