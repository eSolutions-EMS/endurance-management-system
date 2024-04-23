using Not.Application.Ports.CRUD;
using Not.Domain;

namespace Not.Storage.Adapters.Repositories;

/// <summary>
/// Represent a set of operations for root-level entiries in a Tree-like data structure.
/// Implements IReposistory to allow for streamline API, but does not support any Delete operations
/// or Read(Predicate) operations as they are not necessary.
/// </summary>
/// <typeparam name="T">Type of the Root entity</typeparam>
/// <typeparam name="TContext">Type of the state object containing the Root entity</typeparam>
public abstract class RootRepository<T, TContext> : IRepository<T>
    where T : DomainEntity
    where TContext : class, IRootStore<T>, new()
{
    private readonly IStore<TContext> _store;

    public RootRepository(IStore<TContext> store)
    {
        _store = store;
    }

    public async Task<T> Create(T entity)
    {
        var context = await _store.Load();

        context.Root = entity;
        await _store.Commit(context);

        return entity;
    }

    public async Task<T?> Read(int _)
    {
        var context = await _store.Load();
        return context.Root;
    }

    public async Task<T> Update(T entity)
    {
        var context = await _store.Load();

        context.Root = entity;
        await _store.Commit(context);

        return context.Root;
    }
    public Task<T> Delete(int id)
    {
        throw NotImplemented();
    }

    public Task<T> Delete(Predicate<T> filter)
    {
        throw NotImplemented();
    }

    public Task<T> Delete(T entity)
    {
        throw NotImplemented();
    }

    public Task<IEnumerable<T>> Read(Predicate<T> filter)
    {
        throw NotImplemented();
    }

    private Exception NotImplemented()
    {
        return new Exception("Only Create, Read and Update operations are implemented for Root entities.");
    }
}
