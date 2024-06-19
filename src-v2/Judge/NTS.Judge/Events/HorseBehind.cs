using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using NTS.Domain.Setup.Entities;


namespace NTS.Judge.Events;
public class HorseBehind : INotSetBehind<Horse>
{
    private readonly IRepository<Horse> _horseRepository;
    private Horse? _horse;

    public HorseBehind(IRepository<Horse> horseRepository)
    {
        _horseRepository = horseRepository;
    }
    public Task<IEnumerable<Horse>> GetAll()
    {
        return _horseRepository.ReadAll();
    }
    public async Task<Horse> Create(Horse entity)
    {
        _horse = await _horseRepository.Create(entity);
        return _horse;
    }

    public async Task<Horse> Update(Horse entity)
    {
        return await _horseRepository.Update(entity);
    }

    public async Task<Horse> Delete(Horse entity)
    {
        return await _horseRepository.Delete(entity);
    }
}
