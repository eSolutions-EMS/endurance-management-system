using Not.Conventions;
using Not.Domain;

namespace Not.Application.Behinds;

public interface ICreateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Create(T entity);
}
