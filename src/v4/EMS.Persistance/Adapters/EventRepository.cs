using Common.Application.CRUD.Parents;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class EventRepository : RepositoryBase<Event>, IParentRepository<Official>
{
    private readonly IState _state;

    public EventRepository(IState state)
    {
        _state = state;
    }

    public override Task<Event> Create(Event entity)
    {
        _state.Event = entity;
        return Task.FromResult(entity);
    }

    public override Task<Event?> Read(int id)
    {
        if (_state.Event == null)
        {
            return Task.FromResult<Event?>(null);
        }
        return Task.FromResult<Event?>(Clone(_state.Event));
    }

    public override Task<Event> Update(Event @event)
    {
        ThrowHelper.ThrowIfNull(_state.Event);

        PreserveChildrenDuringUpdate(@event);
        _state.Event = @event;
        return Task.FromResult(@event);
    }

    public Task<Official> Create(int parentId, Official child)
    {
        ThrowHelper.ThrowIfNull(_state.Event);

        _state.Event.Add(child);
        return Task.FromResult(child);
    }

    public Task Delete(int parentId, Official child)
    {
        ThrowHelper.ThrowIfNull(_state.Event);

        _state.Event.Remove(child);
        return Task.FromResult(child);
    }

    public Task<Official> Update(Official child)
    {
        var existing = _state.Officials.Find(x => x == child);
        ThrowHelper.ThrowIfNull(existing);

        _state.Event.Update(child);
        return Task.FromResult(child);
    }

    protected override void PreserveChildrenDuringUpdate(Event entity)
    {
        foreach (var official in _state.Event!.Officials)
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