using Not.Injection;

namespace NTS.Judge.Blazor.Ports;

public interface IDocumentBehind : ITransientService
{
    Task CreateHandout(int number);
    Task CreateRanklist(int classificationId);
}