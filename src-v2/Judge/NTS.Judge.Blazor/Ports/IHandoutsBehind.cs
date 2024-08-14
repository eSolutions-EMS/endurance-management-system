using Not.Blazor.Ports.Behinds;
using Not.Injection;
using Not.Startup;
using NTS.Domain.Core.Entities;

namespace NTS.Judge.Blazor.Ports;

public interface IHandoutsBehind : IStartupInitializer, IObservableBehind, ISingletonService
{
    Event? EnduranceEvent { get; }
    IEnumerable<Official> Officials { get; }
    IReadOnlyList<Handout> Handouts { get; }
    Task<IEnumerable<Handout>> PopAll();
}
