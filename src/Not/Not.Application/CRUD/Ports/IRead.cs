using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IRead<T> : ITransient
    where T : AggregateRoot
{
    Task<T?> Read(Predicate<T> filter);
    Task<T?> Read(int id);
    Task<IEnumerable<T>> ReadAll();
    Task<IEnumerable<T>> ReadAll(Predicate<T> filter);
}
