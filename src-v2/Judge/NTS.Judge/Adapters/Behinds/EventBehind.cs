using Not.Application.Ports.CRUD;
using Not.Domain;
using NTS.Domain.Core.Entities;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class EventBehind : IEventBehind
{
    private readonly IRepository<Domain.Setup.Entities.Event> _setupRepository;
    private readonly IRepository<Event> _coreEventRespository;
    private readonly IRepository<Official> _coreOfficialRepository;

    public EventBehind(
        IRepository<Domain.Setup.Entities.Event> setupRepository,
        IRepository<Event> coreEventRespository,
        IRepository<Official> coreOfficialRepository)
    {
        _setupRepository = setupRepository;
        _coreEventRespository = coreEventRespository;
        _coreOfficialRepository = coreOfficialRepository;
    }

    public async Task Start()
    {
        var setupEvent = await _setupRepository.Read(0);
        if (setupEvent == null)
        {
            // TODO: Create ValidationException containing localization logic and inherit form it in DomainException. Use that here instead
            throw new DomainException("Cannot start - event is not configured");
        }
        await CreateEvent(setupEvent);
        await CreateOfficials(setupEvent.Officials);
    }

    private async Task CreateEvent(Domain.Setup.Entities.Event setupEvent)
    {
        var competitionStartTimes = setupEvent.Competitions.Select(x => x.StartTime);
        var startDate = competitionStartTimes.First();
        var endDate = competitionStartTimes.Last();

        var @event = new Event(setupEvent.Country, setupEvent.Place, "", startDate, endDate, null, null, null); // TODO: fix city and place
        await _coreEventRespository.Create(@event);
    }

    private async Task CreateOfficials(IEnumerable<Domain.Setup.Entities.Official> setupOfficials)
    {
        foreach (var setupOfficial in setupOfficials)
        {
            var official = new Official(setupOfficial.Person, setupOfficial.Role);
            await _coreOfficialRepository.Create(official);
        }
    }
}
