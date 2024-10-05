using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Safe;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Events;

public class AthleteBehind : INotSetBehind<Athlete>
{
    private readonly IRepository<Athlete> _athleteRepository;

    public AthleteBehind(IRepository<Athlete> athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    Task<IEnumerable<Athlete>> SafeGetAll()
    {
        return _athleteRepository.ReadAll();
    }

    Task<Athlete> SafeCreate(Athlete entity)
    {
        return _athleteRepository.Create(entity);
    }

    Task<Athlete> SafeUpdate(Athlete entity)
    {
        return _athleteRepository.Update(entity);
    }

    Task<Athlete> SafeDelete(Athlete entity)
    {
        return _athleteRepository.Delete(entity);
    }

    #region SafePattern
    
    public async Task<IEnumerable<Athlete>> GetAll()
    {
        return await SafeHelper.Run(SafeGetAll) ?? [];
    }

    public async Task<Athlete> Create(Athlete athlete)
    {
        return await SafeHelper.Run(() => SafeCreate(athlete)) ?? athlete;
    }

    public async Task<Athlete> Update(Athlete athlete)
    {
        return await SafeHelper.Run(() => SafeUpdate(athlete)) ?? athlete;
    }

    public async Task<Athlete> Delete(Athlete athlete)
    {
        return await SafeHelper.Run(() => SafeDelete(athlete)) ?? athlete;
    }

    #endregion
}
