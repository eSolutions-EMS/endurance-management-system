using Not.Blazor.Ports;
using Not.Injection;
using Not.Startup;
using NTS.Domain.Core.Entities.ParticipationAggregate;
using NTS.Domain.Core.Objects.Documents;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IHandoutsBehind : IStartupInitializer, IObservableBehind, ISingleton
{
    IReadOnlyList<HandoutDocument> Documents { get; }
    Task Delete(IEnumerable<HandoutDocument> documents);
}

public interface ICreateHandout
{
    Task Create(int number);
    Task<IEnumerable<Combination>> GetCombinations();
}
