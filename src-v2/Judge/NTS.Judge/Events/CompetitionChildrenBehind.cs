using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Events;
public class CompetitionChildrenBehind : INotBehindParent<Contestant>, INotBehindParent<Loop>, INotBehindWithChildren<Competition>
{
    private readonly IRead<Competition> _competitionReader;
    private readonly IParentRepository<Contestant> _contestantParentRepository;
    private readonly IParentRepository<Loop> _loopParentRepository;
    public Competition? _competition;

    public CompetitionChildrenBehind(IRead<Competition> competitionReader, IParentRepository<Contestant> contestantRepository, IParentRepository<Loop> loopRepository) 
    {
        _competitionReader = competitionReader;
        _contestantParentRepository = contestantRepository;
        _loopParentRepository = loopRepository;
    }

    IEnumerable<Contestant> INotBehindParent<Contestant>.Children => _competition?.Contestants ?? Enumerable.Empty<Contestant>();
    IEnumerable<Loop> INotBehindParent<Loop>.Children => _competition?.Loops ?? Enumerable.Empty<Loop>();

    public async Task<Contestant> Create(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Add(entity);
        await _contestantParentRepository.Create(_competition.Id, entity);
        return entity;
    }

    public async Task<Contestant> Update(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Update(entity);
        await _contestantParentRepository.Update(_competition.Id, entity);
        return entity;
    }

    public async Task<Contestant> Delete(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Remove(entity);
        await _contestantParentRepository.Delete(_competition.Id, entity);
        return entity;
    }

    public async Task<Loop> Create(Loop entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Update(entity);
        await _loopParentRepository.Update(_competition.Id, entity);
        return entity;
    }

    public async Task<Loop> Update(Loop entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Update(entity);
        await _loopParentRepository.Update(_competition.Id, entity);
        return entity;
    }

    public async Task<Loop> Delete(Loop entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        _competition.Update(entity);
        await _loopParentRepository.Update(_competition.Id, entity);
        return entity;
    }

    async Task INotBehindWithChildren<Competition>.Initialize(int id)
    {
        var competition = await _competitionReader.Read(id);
        if (competition != null)
        {
            _competition = competition;
        }
    }
}
