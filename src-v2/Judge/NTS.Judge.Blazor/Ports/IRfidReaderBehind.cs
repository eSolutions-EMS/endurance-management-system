using Not.Injection;
using Not.Structures;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IRfidReaderBehind : ISingletonService
{
    void StartReading();
    void StopReading();
    bool IsConnected();
}
