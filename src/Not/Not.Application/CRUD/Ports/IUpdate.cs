using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IUpdate<T> : ITransient
    where T : AggregateRoot
{
    Task Update(T entity);
}
