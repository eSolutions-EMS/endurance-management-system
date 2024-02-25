using Not.Conventions;
using Not.Domain;

namespace Not.Application.CRUD;

public interface ICreate<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Create(T entity);
}
