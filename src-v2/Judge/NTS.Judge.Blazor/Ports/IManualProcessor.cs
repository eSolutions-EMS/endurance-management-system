using Not.Injection;

namespace NTS.Judge.Blazor.Ports;

public interface IManualProcessor : ISingletonService
{
    Task Process(Timestamp timestamp);
}
