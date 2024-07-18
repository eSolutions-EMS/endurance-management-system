using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Domain;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;
using static MudBlazor.CategoryTypes;

namespace NTS.Judge.Events;
public class CombinationBehind : INotSetBehind<Combination>
{
    private readonly IRepository<Combination> _combinationRepository;
    private Combination? _combination;

    public CombinationBehind(IRepository<Combination> combinationRepository)
    {
        _combinationRepository = combinationRepository;
    }

    public Task<IEnumerable<Combination>> GetAll()
    {
        return _combinationRepository.ReadAll();
    }

    public async Task<Combination> Create(Combination entity)
    {
        _combination = await _combinationRepository.Create(entity);
        return _combination;
    }

    public async Task<Combination> Update(Combination entity)
    {
        return await _combinationRepository.Update(entity);
    }

    public async Task<Combination> Delete(Combination entity)
    {
        return await _combinationRepository.Delete(entity);
    }
}
