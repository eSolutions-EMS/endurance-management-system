using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

public interface ICreateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Create(T entity);
}
