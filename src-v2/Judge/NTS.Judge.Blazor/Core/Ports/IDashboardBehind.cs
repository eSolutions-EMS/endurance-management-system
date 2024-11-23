using Not.Injection;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IDashboardBehind : ITransient
{
    Task Start();
    Task<bool> IsEnduranceEventStarted();
}
