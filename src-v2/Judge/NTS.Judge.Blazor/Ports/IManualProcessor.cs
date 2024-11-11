using Not.Injection;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IManualProcessor : ISingleton
{
    Task Process(Timestamp timestamp);
}
