using Not.Conventions;
using Not.Domain;

namespace Not.Application.Ports.Behinds;

public interface ICreateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Create(T entity);
}
