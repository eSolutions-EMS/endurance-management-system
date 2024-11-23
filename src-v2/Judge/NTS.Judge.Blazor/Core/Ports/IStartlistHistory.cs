using Not.Blazor.Ports;
using Not.Injection;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IStartlistHistory : IObservableBehind, ISingleton
{
    IReadOnlyList<Start> History { get; }
}
