using Common.Conventions;
using EMS.Domain.Objects;
using EMS.Domain.Ports;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup;

public class SetupPageModel : ISetupPageModel
{
    private readonly IRepository<Event> eventRepository;

    public SetupPageModel(IRepository<Event> eventRepository)
    {
        this.eventRepository = eventRepository;
        this.Countries = new()
        {
            new Country("BG", "Bulgaria"),
            new Country("US", "United States of America"),
            new Country("GB", "Great Britan"),
        };
    }


    public Event? Event { get; private set; }

    public List<Country> Countries { get; }

    public async Task  Read()
    {
        //var result = await this.eventRepository.Read(0);
        //this.Event = result.Data;
    }

    public async Task Create()
    {
        //var result = await this.eventRepository.Create(this.Event);
    }
}

public interface ISetupPageModel : ITransientService
{
    Event? Event { get; }
    List<Country> Countries { get; }
}
