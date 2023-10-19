using EMS.Domain.Ports;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

// TODO: fix service registration generic isn't necessary on the typed implementation
public class EventRepository<T> : RepositoryBase<T>
    where T : Event
{
    public EventRepository()
    {
        this.Data.Add((T)new Event("place", new Domain.Objects.Country("bg", "Bulgaria")));
    }

    public override Task<T?> Read(int id)
    {
        var @event = this.Data.FirstOrDefault();
        return Task.FromResult(@event);
    }
}
