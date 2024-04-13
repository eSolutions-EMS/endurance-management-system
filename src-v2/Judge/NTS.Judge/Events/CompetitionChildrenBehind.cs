using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Events;
public class CompetitionChildrenBehind : INotBehindParent<Contestant>, INotBehindWithChildren<Competition>
{
    private readonly IRead<Competition> _competitionReader;
    private readonly IParentRepository<Contestant> _contestantParentRepository;
    private Competition? _competition;

    public CompetitionChildrenBehind(IRead<Competition> competitionReader, IParentRepository<Contestant> contestants) 
    {
        _competitionReader = competitionReader;
        _contestantParentRepository = contestants;
    }

    IEnumerable<Contestant> INotBehindParent<Contestant>.Children => _competition?.Contestants ?? Enumerable.Empty<Contestant>();

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

    public async Task<Competition> Read(int id)
    {
        return _competition;
    }
    public Task<Competition> Create(Competition entity)
    {
        throw new NotImplementedException();
    }

    public Task<Competition> Update(Competition entity)
    {
        throw new NotImplementedException();
    }

    public Task<Competition> Delete(Competition entity)
    {
        throw new NotImplementedException();
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
