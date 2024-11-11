using Not.Injection;

namespace NTS.Judge.Blazor.Ports;

public interface IDashboardBehind : ITransient
{
    Task Start();
    Task<bool> IsEnduranceEventStarted();
}
