using Not.Storage.Ports.States;

namespace Not.Storage.Adapters.Repositories;

public abstract class SetRepository<T, TState> : IRepository<T>
    where T : DomainEntity
    where TState : class, ISetState<T>, new()
{
    private readonly IStore<TState> _store;

    public SetRepository(IStore<TState> store)
    {
        _store = store;
    }

    public async Task<T> Create(T entity)
    {
        var state = await _store.Transact();
        state.EntitySet.Add(entity);
        await _store.Commit(state);
        return entity;
    }


    public async Task<T> Delete(int id)
    {
        var state = await _store.Transact();
        var match = state.EntitySet.FirstOrDefault(x => x.Id == id);
        if (match == null)
        {
            return default;
        }
        state.EntitySet.Remove(match);
        await _store.Commit(state);
        return match;
    }

    public async Task<T> Delete(Predicate<T> filter)
    {
        var state = await _store.Transact();
        state.EntitySet.RemoveAll(filter);
        await _store.Commit(state);
        return default!;
    }

    public virtual async Task<T> Delete(T entity)
    {
        var state = await _store.Transact();
        state.EntitySet.Remove(entity);
        await _store.Commit(state);
        return entity;
    }

    public Task<T?> Read(int id)
    {
        throw new NotImplementedException();
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

    public virtual async Task<T> Update(T entity)
    {
        var state = await _store.Transact();

        var index = state.EntitySet.IndexOf(entity);
        state.EntitySet.Remove(entity);
        state.EntitySet.Insert(index, entity);

        await _store.Commit(state);
        return entity;
    }
}
