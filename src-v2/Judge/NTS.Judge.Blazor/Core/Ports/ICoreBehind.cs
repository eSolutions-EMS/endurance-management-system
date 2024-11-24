using Not.Injection;

namespace NTS.Judge.Blazor.Core.Ports;

public interface ICoreBehind : ITransient
{
    Task<bool> IsStarted();
    Task Import(string contents);
}
