using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface ICreate<T> : ITransient
    where T : DomainEntity
{
    Task Create(T entity);
}
