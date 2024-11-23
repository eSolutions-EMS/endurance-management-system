using Not.Injection;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IManualProcessor : ISingleton
{
    Task Process(Timestamp timestamp);
}
