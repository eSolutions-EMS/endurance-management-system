using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Domain;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;
using static MudBlazor.CategoryTypes;

namespace NTS.Judge.Events;
public class LapBehind : INotSetBehind<Lap>
{
    private readonly IRepository<Lap> _lapRepository;
    private Lap? _lap;

    public LapBehind(IRepository<Lap> loopRepository)
    {
        _lapRepository = loopRepository;
    }
    public Task<IEnumerable<Lap>> GetAll()
    {
        return _lapRepository.ReadAll();
    }
    public async Task<Lap> Create(Lap entity)
    {
        _lap = await _lapRepository.Create(entity);
        return _lap;
    }

    public async Task<Lap> Update(Lap entity)
    {
        return await _lapRepository.Update(entity);
    }

    public async Task<Lap> Delete(Lap entity)
    {
        return await _lapRepository.Delete(entity);
    }
}
