using NTS.Domain.Setup.Entities;
using Not.Application.Ports.CRUD;

namespace NTS.Persistence.Adapters;

public class EventRepository : IRepository<Event>
{
    private readonly IStore<State> _store;

    public EventRepository(IStore<State> store)
    {
        _store = store;
    }

    public async Task<Event> Create(Event entity)
    {
        var context = await _store.Load();
        
        context.Event = entity;
        await _store.Commit(context);
        
        return entity;
    }

    public async Task<Event?> Read(int _)
    {
        var context = await _store.Load();
        return context.Event;
    }

    public async Task<Event> Update(Event entity)
    {
        var context = await _store.Load();
        
        context.Event = entity;
        await _store.Commit(context);
        
        return context.Event;
    }
    public Task<Event> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Event> Delete(Predicate<Event> filter)
    {
        throw new NotImplementedException();
    }

    public Task<Event> Delete(Event entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Event>> Read(Predicate<Event> filter)
    {
        throw new NotImplementedException();
    }
}