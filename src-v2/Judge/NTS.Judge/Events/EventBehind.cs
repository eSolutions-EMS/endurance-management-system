using NTS.Domain.Setup.Entities;
using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;

namespace NTS.Judge.Events;

public class EventBehind : INotBehind<Event>, INotBehindParent<Official>, INotBehindParent<Competition>
{
    private readonly IRepository<Event> _eventRepository;
    private Event? _event;

    public EventBehind(IRepository<Event> eventRepository)
    {
        _eventRepository = eventRepository;
    }

    IEnumerable<Official> INotBehindParent<Official>.Children => _event?.Officials ?? Enumerable.Empty<Official>();
    IEnumerable<Competition> INotBehindParent<Competition>.Children => _event?.Competitions ?? Enumerable.Empty<Competition>();

    public async Task<Event?> Read(int id)
    {
        _event = await _eventRepository.Read(id);
        return _event;
    }

    public async Task<Event> Create(Event entity)
    {
        await _eventRepository.Create(entity);
        return _event = entity;
    }

    public async Task<Event> Update(Event entity)
    {
        return await _eventRepository.Update(entity);
    }

    public Task<Event> Delete(Event @event)
    {
        throw new NotImplementedException();
    }

    public async Task<Official> Create(Official child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Add(child);
        await _eventRepository.Update(_event);
        return child;
    }

    public async Task<Official> Delete(Official child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Remove(child);
        await _eventRepository.Update(_event);
        return child;
    }

    public async Task<Official> Update(Official child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Update(child);
        await _eventRepository.Update(_event);
        return child;
    }

    public async Task<Competition> Create(Competition child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Add(child);
        await _eventRepository.Update(_event);
        return child;
    }

    public async Task<Competition> Delete(Competition child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Remove(child);
        await _eventRepository.Update(_event);
        return child;
    }

    public async Task<Competition> Update(Competition child)
    {
        GuardHelper.ThrowIfNull(_event);

        _event.Update(child);
        await _eventRepository.Update(_event);
        return child;
    }
}
