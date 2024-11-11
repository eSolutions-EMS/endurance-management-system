using Not.Injection;
using Not.Structures;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IRfidReaderBehind : ISingleton
{
    void StartReading();
    void StopReading();
    bool IsConnected();
}
