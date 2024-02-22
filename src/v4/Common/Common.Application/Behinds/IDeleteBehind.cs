using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

public interface IDeleteBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Delete(T entity);
}
