using Not.Injection;

namespace NTS.Judge.Blazor.Shared.Components.SidePanels;

public interface ICoreBehind : ISingleton
{
    bool IsStarted { get; }
    Task Import(string contents);
    Task<bool> Start();
}
