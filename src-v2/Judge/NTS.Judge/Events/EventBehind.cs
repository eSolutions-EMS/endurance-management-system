using NTS.Domain.Setup.Entities;
using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;

namespace NTS.Judge.Events;

public class EventBehind : INotBehind<Event>, INotBehindParent<Official>, INotBehindParent<Competition>, INotBehindWithChildren<Event>
{
    private readonly IRepository<Event> _repository;
    private readonly IParentRepository<Official> _officialRepository;
    private readonly IParentRepository<Competition> _competitionRepository;
    private Event? _event;

    public EventBehind(IRepository<Event> events, IParentRepository<Official> officials, IParentRepository<Competition> competitions)
    {
        _repository = events;
        _officialRepository = officials;
        _competitionRepository = competitions;
    }
    IEnumerable<Official> INotBehindParent<Official>.Children => _event?.Officials ?? Enumerable.Empty<Official>();
    IEnumerable<Competition> INotBehindParent<Competition>.Children => _event?.Competitions ?? Enumerable.Empty<Competition>();

    public async Task<Event?> Read(int id)
    {
        var events = await _repository.Read(x => true);
        return _event;
    }

    public async Task<Event> Create(Event entity)
    {
        await _repository.Create(entity);
        return _event = entity;
    }

    public async Task<Event> Update(Event entity)
    {
        return await _repository.Update(entity);
    }

    public Task<Event> Delete(Event @event)
    {
        throw new NotImplementedException();
    }

    public async Task<Official> Create(Official child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Add(child);
        await _officialRepository.Create(_event.Id, child);
        return child;
    }

    public async Task<Official> Delete(Official child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Remove(child);
        return await _officialRepository.Delete(_event.Id, child);
    }

    public async Task<Official> Update(Official child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Update(child);
        return await _officialRepository.Update(child);
    }

    public async Task<Competition> Create(Competition child)
    {
        GuardHelper.ThrowIfNull(_event);
        _event.Add(child); 
        return await _competitionRepository.Create(_event.Id, child);
    }

    public async Task<Competition> Delete(Competition child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Remove(child);
        return await _competitionRepository.Delete(_event.Id, child);
    }

    public async Task<Competition> Update(Competition child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Update(child);
        return await _competitionRepository.Update(child);
    }

    public async Task Initialize(int id)
    {
        _event = await _repository.Read(id);
    }
}
