using Not.Injection;

namespace NTS.Judge.Blazor.Shared.Components.SidePanels;

public interface ICoreBehind : ITransient
{
    Task<bool> IsStarted();
    Task Import(string contents);
    Task<bool> Start();
}
