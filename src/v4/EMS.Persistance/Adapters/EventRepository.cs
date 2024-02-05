using Common.Application.CRUD.Parents;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class EventRepository : RepositoryBase<Event>, IParentRepository<Official>
{
    private readonly IStore _store;

    public EventRepository(IStore store)
    {
        _store = store;
    }

    public override Task<Event> Create(Event entity)
    {
        using var context = _store.GetContext();
        context.Event = entity;
        return Task.FromResult(entity);
    }

    public override Task<Event?> Read(int id)
    {
        using var context = _store.GetContext();
        if (context.Event == null)
        {
            return Task.FromResult<Event?>(null);
        }
        return Task.FromResult<Event?>(context.Event);
    }

    public override Task<Event> Update(Event @event)
    {
        using var context = _store.GetContext();
        ThrowHelper.ThrowIfNull(context.Event);
        
        PreserveChildrenDuringUpdate(context.Event, @event);
        context.Event = @event;
        return Task.FromResult(@event);
    }

    public Task<Official> Create(int parentId, Official child)
    {
        using var context = _store.GetContext();
        ThrowHelper.ThrowIfNull(context.Event);

        context.Event.Add(child);
        return Task.FromResult(child);
    }

    public Task Delete(int parentId, Official child)
    {
        using var context = _store.GetContext();
        ThrowHelper.ThrowIfNull(context.Event);

        context.Event.Remove(child);
        return Task.CompletedTask;
    }

    public Task<Official> Update(Official child)
    {
        using var context = _store.GetContext();
        var existing = context.Officials.Find(x => x == child);
        ThrowHelper.ThrowIfNull(existing);

        context.Event.Update(child);
        return Task.FromResult(child);
    }

    protected override void PreserveChildrenDuringUpdate(Event existing, Event update)
    {
        foreach (var official in existing.Officials)
        {
            update.Add(official);
        }
    }
}