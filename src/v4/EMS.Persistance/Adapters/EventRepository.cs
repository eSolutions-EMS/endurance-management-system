using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class EventRepository : RepositoryBase<Event>
{
    private readonly IState _state;

    public EventRepository(IState state)
    {
        state.Event = new Event("place", new Country("BG", "Bulgaria"));
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
        foreach (var member in @event.Staff)
        {
            var existing = _state.StaffMembers.FirstOrDefault(x => x.Id == member.Id);
            if (existing != null)
            {
                _state.StaffMembers.Remove(existing);
            }
            _state.StaffMembers.Add(member);
        }
        _state.Event = @event;
        return Task.FromResult(@event);
    }

    private Event Clone(Event @event)
    {
        var result = new Event(@event.Id, @event.Place, @event.Country);
        foreach (var staff in @event.Staff)
        {
            result.Add(staff);
        }
        return result;
    }
}