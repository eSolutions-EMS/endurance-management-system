using Not.Blazor.Ports.Behinds;
using Not.Injection;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IStartlistBehind : IObservableBehind, ISingletonService
{
    List<Start> Upcoming { get; }
    List<Start> History { get; }
}
