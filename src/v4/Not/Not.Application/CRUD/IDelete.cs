using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD;

public interface IDelete<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Delete(T entity);
}
