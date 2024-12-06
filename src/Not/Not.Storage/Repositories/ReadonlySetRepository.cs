using Not.Application.CRUD.Ports;
using Not.Domain.Base;
using Not.Storage.States;

namespace Not.Storage.Repositories;

public abstract class ReadonlySetRepository<T, TState> : IRead<T>
    where T : AggregateRoot
    where TState : class, ISetState<T>, new()
{
    protected ReadonlySetRepository(IStore<TState> store)
    {
        Store = store;
    }

    protected IStore<TState> Store { get; }

    public async Task<T?> Read(int id)
    {
        var state = await Store.Readonly();
        return state.EntitySet.FirstOrDefault(x => x.Id == id);
    }

    public async Task<T?> Read(Predicate<T> filter)
    {
        var state = await Store.Readonly();
        return state.EntitySet.FirstOrDefault(x => filter(x));
    }

    public async Task<IEnumerable<T>> ReadAll()
    {
        var state = await Store.Readonly();
        return state.EntitySet;
    }

    public async Task<IEnumerable<T>> ReadAll(Predicate<T> filter)
    {
        var state = await Store.Readonly();
        return state.EntitySet.Where(x => filter(x));
    }
}
