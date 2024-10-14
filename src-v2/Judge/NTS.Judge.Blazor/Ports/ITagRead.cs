using Not.Blazor.Ports.Behinds;
using Not.Injection;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;
public interface ITagRead : ISingletonService, IDisposable, IObservableBehind
{
    List<RfidSnapshot> GetSnapshots();
    List<string> GetOutputMessages();
    void ReadTags(bool enabled);
    void TagWasProcessed(RfidSnapshot snapshot);
    bool IsProcessRunning();
}
