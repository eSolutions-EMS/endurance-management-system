using Not.Domain.Base;

namespace Not.Application.CRUD.Ports;

public interface IRepository<T> : ICreate<T>, IRead<T>, IUpdate<T>, IDelete<T>
    where T : DomainEntity
{
    Task<IEnumerable<T>> ReadAll();
    Task<IEnumerable<T>> ReadAll(Predicate<T> filter);
    Task<T?> Read(Predicate<T> filter);
    Task Delete(int id);
    Task Delete(Predicate<T> filter);
}
