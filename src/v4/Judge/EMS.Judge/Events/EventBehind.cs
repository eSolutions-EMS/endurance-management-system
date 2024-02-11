using Common.Application.Behinds;
using Common.Application.CRUD;
using Common.Application.CRUD.Parents;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Events;

public class EventBehind : INotBehind<Event>, INotBehindParent<Official>
{
    private readonly IRepository<Event> _repository;
    private readonly IParentRepository<Official> _officialRepository;
    private Event? _event;

    public EventBehind(IRepository<Event> events, IParentRepository<Official> officials)
    {
        _repository = events;
        _officialRepository = officials;
    }

    public IEnumerable<Official> Children => _event?.Officials ?? Enumerable.Empty<Official>();

    public async Task<Event?> Read(int id)
    {
        var events = await _repository.Read(x => true);
        _event = events.FirstOrDefault();
        return _event;
    }

    public async Task<Event> Create(Event entity)
    {
        return await _repository.Create(entity);
    }

    public async Task<Event> Update(Event entity)
    {
        return await _repository.Update(entity);
    }

    public async Task<Event> Delete(int id)
    {
        return await _repository.Delete(id);
    }

    public async Task<Official> Create(Official child)
    {
        ThrowHelper.ThrowIfNull(_event);

        _event.Add(child);
        await _officialRepository.Create(_event.Id, child);
        return child;
    }

    public async Task<Official> Delete(Official child)
    {
        ThrowHelper.ThrowIfNull(_event);

        _event.Remove(child);
        return await _officialRepository.Delete(_event.Id, child);
    }

    public async Task<Official> Update(Official child)
    {
        ThrowHelper.ThrowIfNull(_event);

        _event.Update(child);
        await _officialRepository.Update(child);
        return child;
    }
}
