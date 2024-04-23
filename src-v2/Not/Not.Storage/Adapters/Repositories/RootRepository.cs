using Not.Application.Ports.CRUD;
using Not.Domain;

namespace Not.Storage.Adapters.Repositories;

/// <summary>
/// Represent a set of operations for root-level entiries in a Tree-like data structure.
/// Implements IReposistory to allow for streamline API, but does not support any Delete operations
/// or Read(Predicate) operations as they are not necessary.
/// </summary>
/// <typeparam name="T">Type of the Root entity</typeparam>
/// <typeparam name="TState">Type of the state object containing the Root entity</typeparam>
public abstract class RootRepository<T, TState> : IRepository<T>
    where T : DomainEntity
    where TState : class, IRootStore<T>, new()
{
    private readonly IStore<TState> _store;

    public RootRepository(IStore<TState> store)
    {
        _store = store;
    }

    public async Task<T> Create(T entity)
    {
        var state = await _store.Load();

        state.Root = entity;
        await _store.Commit(state);

        return entity;
    }

    public async Task<T?> Read(int _)
    {
        var state = await _store.Load();
        return state.Root;
    }

    public async Task<T> Update(T entity)
    {
        var state = await _store.Load();

        state.Root = entity;
        await _store.Commit(state);

        return state.Root;
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
