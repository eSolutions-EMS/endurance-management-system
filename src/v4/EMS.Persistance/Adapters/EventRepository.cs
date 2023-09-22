using Common.Domain;
using EMS.Domain.Core;
using EMS.Domain.Core.Ports;

namespace EMS.Persistence.Adapters;

public class EventRepository : IEventRepository
{
    public Task<Event> Get(Guid id)
    {
        return Task.FromResult(new Event { Id = id } );
    }

    public Task ResetState()
    {
        return Task.CompletedTask;
    }
}
