using Common.Domain.Ports;
using EMS.Domain.Setup.Entities;
using System.Runtime.CompilerServices;

namespace EMS.Persistence.Adapters;

// TODO: fix service registration generic isn't necessary on the typed implementation
public class EventRepository : RepositoryBase<Event>
{
    public EventRepository()
    {
        this.Data.Add(new Event("place", new Domain.Objects.Country("bg", "Bulgaria")));
    }

    public override Task<Event?> Read(int id)
    {
        var @event = this.Data.FirstOrDefault();
        return Task.FromResult(@event);
    }
}