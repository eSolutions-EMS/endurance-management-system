using Not.Blazor.Ports.Behinds;
using Not.Injection;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IStartlistBehind : IObservableBehind, ISingletonService
{
    StartList? StartList { get; }
    List<Start> UpcomingStarts { get; }
    List<Start> StartHistory { get; }
}
