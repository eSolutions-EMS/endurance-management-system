using Common.Application.CRUD.Parents;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class OfficialRepository : IParentRepository<Official>
{
    private readonly IState _state;

    public OfficialRepository(IState state)
    {
        _state = state;
    }

    public Task<Official> Create(int parentId, Official child)
    {
        ThrowHelper.ThrowIfNull(_state.Event);

        _state.Event.Add(child);
        return Task.FromResult(child);
    }

    public Task Delete(int parentId, Official child)
    {
        ThrowHelper.ThrowIfNull(_state.Event);

        _state.Event.Remove(child);
        return Task.FromResult(child);
    }

    public Task<Official> Update(Official child)
    {
        var existing = _state.Officials.Find(x => x == child);
        ThrowHelper.ThrowIfNull(existing);

        _state.Event.Update(child);
        return Task.FromResult(child);
    }
}
