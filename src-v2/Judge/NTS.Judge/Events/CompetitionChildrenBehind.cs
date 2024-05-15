using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Events;
public class CompetitionChildrenBehind : INotBehindParent<Contestant>, INotBehindParent<Phase>, INotBehindWithChildren<Competition>
{
    private readonly IRead<Competition> _competitionReader;
    private readonly IRepository<Competition> _competitionRepository;
    private Competition? _competition;

    public CompetitionChildrenBehind(IRead<Competition> competitionReader, IRepository<Competition> competitionRepository) 
    {
        _competitionReader = competitionReader;
        _competitionRepository = competitionRepository;
    }

    IEnumerable<Contestant> INotBehindParent<Contestant>.Children => _competition?.Contestants ?? Enumerable.Empty<Contestant>();
    IEnumerable<Phase> INotBehindParent<Phase>.Children => _competition?.Phases ?? Enumerable.Empty<Phase>();

    public async Task<Contestant> Create(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Add(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Contestant> Update(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Update(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Contestant> Delete(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Remove(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Phase> Create(Phase entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Add(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Phase> Update(Phase entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Update(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Phase> Delete(Phase entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Remove(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task Initialize(int id)
    {
        _competition = await _competitionReader.Read(id);
    }
}
