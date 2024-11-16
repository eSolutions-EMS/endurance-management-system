using Not.Application.CRUD.Ports;
using Not.Domain.Base;
using Not.Storage.Ports.States;

namespace Not.Storage.Adapters.Repositories;

public abstract class SetRepository<T, TState> : IRepository<T>
    where T : DomainEntity
    where TState : class, ISetState<T>, new()
{
    readonly IStore<TState> _store;

    public SetRepository(IStore<TState> store)
    {
        _store = store;
    }

    public async Task Create(T entity)
    {
        var state = await _store.Transact();
        state.EntitySet.Add(entity);
        await _store.Commit(state);
    }

    public async Task Delete(int id)
    {
        var state = await _store.Transact();
        state.EntitySet.RemoveAll(x => x.Id == id);
        await _store.Commit(state);
    }

    public async Task Delete(Predicate<T> filter)
    {
        var state = await _store.Transact();
        state.EntitySet.RemoveAll(filter);
        await _store.Commit(state);
    }

    public virtual async Task Delete(T entity)
    {
        var state = await _store.Transact();
        state.EntitySet.Remove(entity);
        await _store.Commit(state);
    }

    public async Task Delete(IEnumerable<T> entities)
    {
        var state = await _store.Transact();
        foreach (var entity in entities)
        {
            state.EntitySet.Remove(entity);
        }
        await _store.Commit(state);
    }

    public async Task<T?> Read(int id)
    {
        var state = await _store.Readonly();
        return state.EntitySet.FirstOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<T>> ReadAll()
    {
        var state = await _store.Readonly();
        return state.EntitySet;
    }

    public async Task<IEnumerable<T>> ReadAll(Predicate<T> filter)
    {
        var state = await _store.Readonly();
        return state.EntitySet.Where(x => filter(x));
    }

    public async Task<T?> Read(Predicate<T> filter)
    {
        var state = await _store.Readonly();
        return state.EntitySet.FirstOrDefault(x => filter(x));
    }

    public virtual async Task Update(T entity)
    {
        var state = await _store.Transact();

        var index = state.EntitySet.IndexOf(entity);
        state.EntitySet.Remove(entity);
        state.EntitySet.Insert(index, entity);

        await _store.Commit(state);
    }
}
