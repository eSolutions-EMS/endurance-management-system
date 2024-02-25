using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

public interface IUpdateBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Update(T entity);
}
