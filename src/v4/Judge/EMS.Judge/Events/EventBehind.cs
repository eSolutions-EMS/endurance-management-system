using Common.Application;
using Common.Application.CRUD;
using Common.Application.CRUD.Parents;
using Common.Domain;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Events;

public class EventBehind : IParentBehind<Official>
{
    private readonly IRepository<Event> _repository;
    private readonly IParentRepository<Official> _officialRepository;
    private Event? _event;

    public EventBehind(IRepository<Event> eventReader, IParentRepository<Official> officialRepository)
    {
        _repository = eventReader;
        _officialRepository = officialRepository;
    }

    public DomainEntity? Parent => _event;
    public IEnumerable<Official> Children => _event?.Officials ?? Enumerable.Empty<Official>();

    public async Task Init(int parentId)
    {
        var events = await _repository.Read(x => true);
        _event = events.FirstOrDefault();
    }

    public async Task<Official> Create(Official child)
    {
        ThrowHelper.ThrowIfNull(_event);

        _event.Add(child);
        await _officialRepository.Create(_event.Id, child);
        return child;
    }

    public async Task Delete(Official child)
    {
        ThrowHelper.ThrowIfNull(_event);

        _event.Remove(child);
        await _officialRepository.Delete(_event.Id, child);
    }

    public async Task<Official> Update(Official child)
    {
        ThrowHelper.ThrowIfNull(_event);

        _event.Update(child);
        await _officialRepository.Update(child);
        return child;
    }
}
