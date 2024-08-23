using Not.Blazor.Ports.Behinds;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IStartlistBehind : IObservableBehind
{
    IEnumerable<PhaseStart> Startlist { get; }
    IEnumerable<IGrouping<double, PhaseStart>> StartlistByDistance { get; }
}
