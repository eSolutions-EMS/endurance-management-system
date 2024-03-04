using Not.Injection;
using Not.Domain;

namespace Not.Blazor.Ports.Behinds;

public interface IUpdateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Update(T entity);
}
