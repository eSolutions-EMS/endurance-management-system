using Not.Blazor.Ports.Behinds;
using Not.Injection;
using Not.Startup;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IHandoutsBehind : IStartupInitializer, IObservableBehind, ISingletonService
{
    IReadOnlyList<HandoutDocument> Documents { get; }
    Task<IEnumerable<HandoutDocument>> PopAll();
    Task<Participation?> GetParticipation(int id);
}
