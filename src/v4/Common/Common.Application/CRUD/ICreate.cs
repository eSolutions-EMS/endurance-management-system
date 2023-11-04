using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD;

public interface ICreate<T> : ITransientService
    where T : DomainEntity
{
    Task Create(T entity);
}
