using Not.Injection;
using Not.Domain;

namespace Not.Blazor.Ports.Behinds;

public interface IReadAllBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<IEnumerable<T>> GetAll();
}
