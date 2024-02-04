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
        _store.Event = entity;
        return Task.FromResult(entity);
    }

    public override Task<Event?> Read(int id)
    {
        if (_store.Event == null)
        {
            return Task.FromResult<Event?>(null);
        }
        return Task.FromResult<Event?>(Clone(_store.Event));
    }

    public override Task<Event> Update(Event @event)
    {
        ThrowHelper.ThrowIfNull(_store.Event);

        PreserveChildrenDuringUpdate(@event);
        _store.Event = @event;
        return Task.FromResult(@event);
    }

    public Task<Official> Create(int parentId, Official child)
    {
        ThrowHelper.ThrowIfNull(_store.Event);

        _store.Event.Add(child);
        return Task.FromResult(child);
    }

    public Task Delete(int parentId, Official child)
    {
        ThrowHelper.ThrowIfNull(_store.Event);

        _store.Event.Remove(child);
        return Task.FromResult(child);
    }

    public Task<Official> Update(Official child)
    {
        var existing = _store.Officials.Find(x => x == child);
        ThrowHelper.ThrowIfNull(existing);

        _store.Event.Update(child);
        return Task.FromResult(child);
    }

    protected override void PreserveChildrenDuringUpdate(Event entity)
    {
        foreach (var official in _store.Event!.Officials)
        {
            entity.Add(official);
        }
    }

    private Event Clone(Event @event)
    {
        var result = new Event(@event.Id, @event.Place, @event.Country);
        foreach (var staff in @event.Officials)
        {
            result.Add(staff);
        }
        return result;
    }
}