using Not.Blazor.Ports.Behinds;
using Not.Injection;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IStartlistBehind : ISingletonService
{
    Task Initialize();
    StartList? StartList { get; }
    IEnumerable<Start> UpcomingStarts { get; }
    IEnumerable<Start> StartHistory { get; }
}
