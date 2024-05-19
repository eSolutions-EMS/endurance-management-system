using Not.Injection;

namespace NTS.Judge.Blazor.Ports;

public interface IEventBehind : ITransientService
{
    Task Start();
}