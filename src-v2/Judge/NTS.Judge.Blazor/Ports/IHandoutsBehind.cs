using Not.Blazor.Ports.Behinds;
using Not.Injection;
using Not.Startup;
using NTS.Domain.Core.Entities.ParticipationAggregate;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IHandoutsBehind : IStartupInitializer, IObservableBehind, ISingletonService
{
    IReadOnlyList<HandoutDocument> Documents { get; }
    Task Delete(IEnumerable<HandoutDocument> documents);
}

public interface ICreateHandout
{
    Task Create(int number);
    Task<IEnumerable<Tandem>> GetCombinations();
}
