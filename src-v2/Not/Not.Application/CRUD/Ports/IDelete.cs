using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IDelete<T> : ITransient
    where T : DomainEntity
{
    Task Delete(T entity);
    Task Delete(IEnumerable<T> entities);
}
