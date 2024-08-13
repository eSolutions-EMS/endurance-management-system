using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Concurrency;
using Not.Events;
using Not.Exceptions;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Events;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class HandoutsBehind : ObservableBehind, IHandoutsBehind
{
    private readonly ConcurrentList<HandoutDocument> _handouts = new();
    private readonly IRepository<Participation> _participationRepository;
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<Official> _officialRepository;

    public HandoutsBehind(
        IRepository<Participation> participationRepository,
        IRepository<Event> eventRepository,
        IRepository<Official> officialRepository)
    {
        _participationRepository = participationRepository;
        _eventRepository = eventRepository;
        _officialRepository = officialRepository;
    }

    public IReadOnlyList<HandoutDocument> Handouts => _handouts.AsReadOnly();

    public IEnumerable<HandoutDocument> PopAll()
    {
        return _handouts.PopAll();
    }

    public void RunAtStartup()
    {
        EventHelper.Subscribe<PhaseCompleted>(CreateHandout);
    }

    public async void CreateHandout(PhaseCompleted phaseCompleted)
    {
        var participation = await _participationRepository.Read(x => x.Tandem.Number == phaseCompleted.Number);
        var @event = await _eventRepository.Read(0);
        var officials = await _officialRepository.ReadAll();

        GuardHelper.ThrowIfDefault(@event);
        GuardHelper.ThrowIfDefault(participation);

        var handout = new HandoutDocument(participation, @event, officials);
        _handouts.Add(handout);
    }

    public override Task Initialize()
    {
        return Task.CompletedTask;
    }
}
