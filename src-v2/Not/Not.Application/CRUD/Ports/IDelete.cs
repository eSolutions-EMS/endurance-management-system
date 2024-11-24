using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IDelete<T> : ITransient
    where T : AggregateRoot
{
    Task Delete(T entity);
    Task Delete(IEnumerable<T> entities);
}
