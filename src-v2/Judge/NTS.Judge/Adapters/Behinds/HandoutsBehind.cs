using Not.Application.Behinds.Adapters;
using Not.Application.CRUD.Ports;
using Not.Concurrency;
using Not.Exceptions;
using Not.Safe;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Entities.ParticipationAggregate;
using NTS.Domain.Core.Objects;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class HandoutsBehind : ObservableListBehind<HandoutDocument>, IHandoutsBehind, ICreateHandout
{
    readonly SemaphoreSlim _semaphore = new(1);
    readonly IRepository<Handout> _handoutRepository;
    readonly IRepository<Participation> _participations;
    readonly IRepository<EnduranceEvent> _events;
    readonly IRepository<Official> _officials;

    public HandoutsBehind(
        IRepository<Handout> handouts,
        IRepository<Participation> participations,
        IRepository<EnduranceEvent> events,
        IRepository<Official> officials
    )
    {
        _handoutRepository = handouts;
        _participations = participations;
        _events = events;
        _officials = officials;
    }

    public IReadOnlyList<HandoutDocument> Documents => ObservableList;

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var handouts = await _handoutRepository.ReadAll();
        var enduranceEvent = await _events.Read(0);
        var officials = await _officials.ReadAll();
        GuardHelper.ThrowIfDefault(enduranceEvent);

        var documents = handouts.Select(handout => new HandoutDocument(
            handout,
            enduranceEvent,
            officials
        ));
        ObservableList.AddRange(documents);

        return true;
    }

    public void RunAtStartup()
    {
        // TODO: subscribe to updates for Event, Official
        Participation.PhaseCompletedEvent.SubscribeAsync(PhaseCompletedHandler);
    }

    public async Task Delete(IEnumerable<HandoutDocument> documents)
    {
        Task action() => SafeDelete(documents);
        await SafeHelper.Run(action);
    }

    public async Task Create(int number)
    {
        Task action() => SafeCreate(number);
        await SafeHelper.Run(action);
    }

    public async Task<IEnumerable<Combination>> GetCombinations()
    {
        return await SafeHelper.Run(SafeGetCombinations) ?? [];
    }

    async Task SafeCreate(int number)
    {
        var participation = await _participations.Read(x => x.Combination.Number == number);
        GuardHelper.ThrowIfDefault(participation);

        await CreateDocument(participation);
    }

    async Task<IEnumerable<Combination>> SafeGetCombinations()
    {
        return await _participations.ReadAll().Select(x => x.Combination);
    }

    async Task SafeDelete(IEnumerable<HandoutDocument> documents)
    {
        await _semaphore.WaitAsync();

        var ids = documents.Select(x => x.Id);
        await _handoutRepository.Delete(x => ids.Contains(x.Id));
        ObservableList.RemoveRange(documents);

        _semaphore.Release();
    }

    async void PhaseCompletedHandler(PhaseCompleted phaseCompleted)
    {
        await CreateDocument(phaseCompleted.Participation);
    }

    async Task CreateDocument(Participation participation)
    {
        var enduranceEvent = await _events.Read(0);
        var officials = await _officials.ReadAll();
        GuardHelper.ThrowIfDefault(enduranceEvent);

        var handout = new Handout(participation);
        var document = new HandoutDocument(handout, enduranceEvent, officials);

        await _semaphore.WaitAsync();

        await _handoutRepository.Delete(x => x.Participation == participation);
        await _handoutRepository.Create(handout);
        ObservableList.AddOrReplace(document);

        _semaphore.Release();
    }
}
