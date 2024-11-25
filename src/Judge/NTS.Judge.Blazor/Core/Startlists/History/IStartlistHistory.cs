using Not.Blazor.Ports;
using Not.Injection;
using NTS.Domain.Core.Objects.Startlists;

namespace NTS.Judge.Blazor.Core.Startlists.History;

public interface IStartlistHistory : IObservableBehind, ISingleton
{
    IReadOnlyList<StartlistEntry> History { get; }
}
