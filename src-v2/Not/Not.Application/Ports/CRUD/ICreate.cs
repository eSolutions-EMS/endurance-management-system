using Not.Domain;
using Not.Injection;

namespace Not.Application.Ports.CRUD;

public interface ICreate<T> : ITransient
    where T : DomainEntity
{
    Task Create(T entity);
}
