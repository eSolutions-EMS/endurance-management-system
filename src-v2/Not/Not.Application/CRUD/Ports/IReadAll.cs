using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IReadAll<T> : ITransient
    where T : AggregateRoot
{
    Task<IEnumerable<T>> ReadAll();
}
