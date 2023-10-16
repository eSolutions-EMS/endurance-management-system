using Common.Conventions;
using EMS.Domain.Objects;
using EMS.Domain.Ports;
using EMS.Domain.Setup.Entities;
using EMS.Judge.Ports;
using EMS.Judge.Setup.Models;

namespace EMS.Judge.Setup;

public class EventService : IEventCreateService, IEventUpdateService
{
    private readonly IRepository<Event> repository;

    public EventService(IRepository<Event> repository, IJudgeContext context)
    {
        this.repository = repository;
        this.Countries = context.Countries;
    }

    public Event? Event { get; private set; }
    public EventCreateModel CreateModel { get; private set; } = new();
    public IReadOnlyList<Country> Countries { get; }

    public async Task Create()
    {
        var country = this.Countries.First(x => x.Name == this.CreateModel.CountryName);
        var @event = new Event(this.CreateModel.Place!, country);
        var result = await this.repository.Create(@event);
    }

    public async Task Read()
    {
        this.Event = await this.repository.Read(0);
    }

    public async Task Delete()
    {
        await this.repository.Delete(this.Event!);
        this.Event = null;
    }

    public async Task<IEnumerable<Country>> SearchByName(string partial)
    {
        if (partial is null or "")
        {
            return this.Countries;
        }
        return this.Countries.Where(x => x.Name.Contains(partial));
    }
}

public interface IEventCreateService : ICountryAware, ITransientService
{
    EventCreateModel CreateModel { get; }
    Task Create();
}

public interface IEventUpdateService : ICountryAware, ITransientService
{
    Event? Event { get; }
    Task Read();
    Task Delete();
}

public interface ICountryAware
{
    IReadOnlyList<Country> Countries { get; }
    Task<IEnumerable<Country>> SearchByName(string partial);
}
