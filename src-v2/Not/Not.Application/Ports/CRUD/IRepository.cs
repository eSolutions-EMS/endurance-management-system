using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IRepository<T> : ICreate<T>, IRead<T>, IUpdate<T>, IDelete<T>
    where T : DomainEntity
{
    Task<IEnumerable<T>> ReadAll();
    Task<IEnumerable<T>> ReadAll(Predicate<T> filter);
    Task<T?> Read(Predicate<T> filter);
    Task<T> Delete(int id);
    Task<T> Delete(Predicate<T> filter);
}
