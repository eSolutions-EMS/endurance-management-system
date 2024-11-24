using Not.Injection;

namespace NTS.Judge.Blazor.Shared.EnduranceEventState;

public interface ICoreBehind : ITransient
{
    Task<bool> IsStarted();
    Task Import(string contents);
    Task Start();
}
