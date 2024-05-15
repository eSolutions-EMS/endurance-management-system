using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Domain;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;
using static MudBlazor.CategoryTypes;


namespace NTS.Judge.Events;
public class PhaseBehind : INotBehindParent<Lap>, INotBehindWithChildren<Phase>
{
    private readonly IRepository<Phase> _phaseRepository;
    private Phase? _phase;

    public IEnumerable<Lap> Children => Phase.Laps;

    public PhaseBehind(IRepository<Phase> loopRepository)
    {
        _phaseRepository = loopRepository;
    }

    public async Task Initialize(int id)
    {
        _phase = new Phase();
    }
    public async Task<Phase?> Read(int id)
    {
        _phase = await _phaseRepository.Read(id);
        return _phase;
    }
    public async Task<Phase> Create(Phase entity)
    {
        await _phaseRepository.Create(entity);
        return entity;
    }
    public async Task<Phase> Update(Phase entity)
    {
        await _phaseRepository.Update(entity);
        return entity;
    }
    public async Task<Phase> Delete(Phase entity)
    {
        await _phaseRepository.Delete(entity);
        return entity; ;
    }

    public async Task<Lap> Create(Lap child)
    {
        GuardHelper.ThrowIfNull(_phase);

        _phase.Add(child);
        await _phaseRepository.Update(_phase);
        return child;
    }

    public async Task<Lap> Update(Lap child)
    {
        GuardHelper.ThrowIfNull(_phase);

        _phase.Update(child);
        await _phaseRepository.Update(_phase);
        return child;
    }

    public async Task<Lap> Delete(Lap child)
    {
        GuardHelper.ThrowIfNull(_phase);

        _phase.Remove(child);
        await _phaseRepository.Update(_phase);
        return child;
    }
}
