using Not.Injection;
using Not.Structures;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;
public interface IRfidTagReaderBehind : ISingletonService
{
    void StartReading();
    void StopReading();
    bool IsConnected();
    bool IsReading();
}
