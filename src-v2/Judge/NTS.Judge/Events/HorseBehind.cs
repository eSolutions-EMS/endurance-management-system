using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Safe;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Events;

public class HorseBehind : INotSetBehind<Horse>
{
    private readonly IRepository<Horse> _horseRepository;

    public HorseBehind(IRepository<Horse> horseRepository)
    {
        _horseRepository = horseRepository;
    }

    Task<IEnumerable<Horse>> SafeGetAll()
    {
        return _horseRepository.ReadAll();
    }

    Task<Horse> SafeCreate(Horse entity)
    {
        return _horseRepository.Create(entity);
    }

    Task<Horse> SafeUpdate(Horse entity)
    {
        return _horseRepository.Update(entity);
    }

    Task<Horse> SafeDelete(Horse entity)
    {
        return _horseRepository.Delete(entity);
    }

    #region SafePattern

    public async Task<IEnumerable<Horse>> GetAll()
    {
        return await SafeHelper.Run(SafeGetAll) ?? [];
    }

    public async Task<Horse> Create(Horse horse)
    {
        return await SafeHelper.Run(() => SafeCreate(horse)) ?? horse;
    }

    public async Task<Horse> Update(Horse horse)
    {
        return await SafeHelper.Run(() => SafeUpdate(horse)) ?? horse;
    }

    public async Task<Horse> Delete(Horse horse)
    {
        return await SafeHelper.Run(() => SafeDelete(horse)) ?? horse;
    }

    #endregion
}
