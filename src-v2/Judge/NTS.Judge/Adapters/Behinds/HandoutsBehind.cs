using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Collections;
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
    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly IRepository<Handout> _handoutRepository;
    private readonly IRepository<Participation> _participations;
    private readonly IRepository<Event> _events;
    private readonly IRepository<Official> _officials;
    private ConcurrentList<HandoutDocument> _documents = new();

    public HandoutsBehind(
        IRepository<Handout> handouts,
        IRepository<Participation> participations,
        IRepository<Event> events,
        IRepository<Official> officials)
    {
        _handoutRepository = handouts;
        _participations = participations;
        _events = events;
        _officials = officials;
    }

    public IReadOnlyList<HandoutDocument> Documents => _documents.AsReadOnly();

    public void RunAtStartup()
    {
        // TODO: subscribe to updates for Event, Official
        EventHelper.Subscribe<PhaseCompleted>(CreateHandout);
    }

    protected override async Task PerformInitialization()
    {
        var handouts = await _handoutRepository.ReadAll();
        var participations = await _participations.ReadAll(x => handouts.Any(y => y.ParticipationId == x.Id));
        var enduranceEvent = await _events.Read(0);
        var officials = await _officials.ReadAll();
        GuardHelper.ThrowIfDefault(enduranceEvent);

        var documents = handouts.Select(x => new HandoutDocument(
            participations.First(y => y.Id == x.ParticipationId),
            enduranceEvent, 
            officials));
        _documents = new(documents);
        EmitChange();
    }
    
    // TODO: we need a separete list and delete method that executes only after print is initiated,
    // otherwise if the user clicks print and then back the handouts would be deleted
    public async Task<IEnumerable<HandoutDocument>> PopAll() 
    {
        await _semaphore.WaitAsync();

        await _handoutRepository.Delete(x => true);
        var handouts = _documents.PopAll();
        
        _semaphore.Release();
        return handouts;
    }

    public async void CreateHandout(PhaseCompleted phaseCompleted)
    {
        var @event = await _events.Read(0);
        var officials = await _officials.ReadAll();
        GuardHelper.ThrowIfDefault(@event);

        await _semaphore.WaitAsync();

        await HandleExistingHandout(phaseCompleted);

        var handout = new Handout(phaseCompleted.Participation);
        await _handoutRepository.Create(handout);

        var document = new HandoutDocument(phaseCompleted.Participation, @event, officials);
        _documents.Add(document);

        _semaphore.Release();

        EmitChange();
    }

    private async Task HandleExistingHandout(PhaseCompleted phaseCompleted)
    {
        var existing = await _handoutRepository.Read(x => x.ParticipationId == phaseCompleted.Participation.Id);
        if (existing != null)
        {
            await _handoutRepository.Delete(existing);
            _documents.RemoveIfExisting(x => x.Tandem.Number == phaseCompleted.Participation.Tandem.Number);
        }
    }

    public async Task<Participation?> GetParticipation(int id)
    {
        return await _participations.Read(id);
    }
}
