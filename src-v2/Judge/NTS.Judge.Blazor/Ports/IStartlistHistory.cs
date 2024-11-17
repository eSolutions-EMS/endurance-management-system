using Not.Blazor.Ports.Behinds;
using Not.Injection;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IStartlistHistory : IObservableBehind, ISingletonService
{
    IReadOnlyList<Start> History { get; }
}
