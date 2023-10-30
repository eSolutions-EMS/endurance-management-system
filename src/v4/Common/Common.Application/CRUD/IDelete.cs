using Common.Domain;

namespace Common.Application.CRUD;

public interface IDelete<in T>
    where T : DomainEntity
{
    Task Delete(T entity);
}
