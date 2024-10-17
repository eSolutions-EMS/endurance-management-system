using Not.Injection;

namespace NTS.Judge.Blazor.Ports;
public interface IRfidTagReaderBehind : ISingletonService
{
    Task StartReading(bool readFlag);
    void ProcessTags();
}
