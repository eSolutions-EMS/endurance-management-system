using EMS.Domain.Ports;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

// TODO: fix service registration generic isn't necessary on the typed implementation
public class EventRepository<T> : RepositoryBase<T>
    where T : Event
{
    public override Task<T?> Read(int id)
    {
        var @event = this.Data.FirstOrDefault();
        return Task.FromResult(@event);
    }
}
