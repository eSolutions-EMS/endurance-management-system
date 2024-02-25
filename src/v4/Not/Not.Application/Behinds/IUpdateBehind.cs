using Not.Conventions;
using Not.Domain;

namespace Not.Application.Behinds;

public interface IUpdateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Update(T entity);
}
