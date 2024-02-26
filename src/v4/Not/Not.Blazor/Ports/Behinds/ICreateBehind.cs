using Not.Injection;
using Not.Domain;

namespace Not.Blazor.Ports.Behinds;

public interface ICreateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Create(T entity);
}
