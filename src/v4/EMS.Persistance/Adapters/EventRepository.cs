using Common.Domain.Ports;
using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class EventRepository : RepositoryBase<Event>
{
    private readonly IState _state;
    private readonly IRepository<StaffMember> _staffMembers;

    public EventRepository(IState state, IRepository<StaffMember> staffMembers)
    {
        state.Event = new Event("place", new Country("BG", "Bulgaria"));
        _state = state;
        _staffMembers = staffMembers;
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
            _staffMembers.Update(member);
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