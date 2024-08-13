using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Concurrency;
using Not.Events;
using Not.Exceptions;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Events.Participations;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class HandoutsBehind : ObservableBehind, IHandoutsBehind
{
    private ConcurrentList<HandoutDocument> _handouts = new();
    private readonly IRepository<HandoutDocument> _handoutRepository;
    private readonly IRepository<Participation> _participationRepository;
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<Official> _officialRepository;

    public HandoutsBehind(
        IRepository<HandoutDocument> handouts,
        IRepository<Participation> participationRepository,
        IRepository<Event> eventRepository,
        IRepository<Official> officialRepository)
    {
        _handoutRepository = handouts;
        _participationRepository = participationRepository;
        _eventRepository = eventRepository;
        _officialRepository = officialRepository;
    }

    public IReadOnlyList<HandoutDocument> Handouts => _handouts.AsReadOnly();

    public void RunAtStartup()
    {
        EventHelper.Subscribe<PhaseCompleted>(CreateHandout);
    }

    public override async Task Initialize()
    {
        _handouts = new(await _handoutRepository.ReadAll());
        EmitChange();
    }

    public async Task<IEnumerable<HandoutDocument>> PopAll()
    {
        var handouts = _handouts.PopAll();
        await _handoutRepository.Delete(handouts);
        return handouts;
    }

    public async void CreateHandout(PhaseCompleted phaseCompleted)
    {
        var @event = await _eventRepository.Read(0);
        var officials = await _officialRepository.ReadAll();

        GuardHelper.ThrowIfDefault(@event);

        var handout = new HandoutDocument(phaseCompleted.Participation, @event, officials);
        await _handoutRepository.Create(handout);
        _handouts.Add(handout);
    }
}
