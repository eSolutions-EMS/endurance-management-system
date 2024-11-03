using Not.Domain;
using Not.Injection;

namespace Not.Application.Ports.CRUD;

public interface ICreate<T> : ITransientService
    where T : DomainEntity
{
    Task Create(T entity);
}
