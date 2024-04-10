using Not.Application.Ports.CRUD;
using Not.Blazor.Navigation;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Events;
public class CompetitionChildrenBehind : INotBehindParent<Contestant>, INotBehindWithChildren<Competition>
{
    private readonly IRepository<Competition> _competitionRepository;
    private readonly IParentRepository<Contestant> _contestantRepository;
    private Competition _competition;

    public CompetitionChildrenBehind(IRepository<Competition> competitions,IParentRepository<Contestant> contestants)
    {
        _competitionRepository = competitions;
        _contestantRepository = contestants;
    }
    IEnumerable<Contestant> INotBehindParent<Contestant>.Children => _competition?.Contestants ?? Enumerable.Empty<Contestant>();

    public async Task<Contestant> Create(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        await _contestantRepository.Create(_competition.Id, entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Contestant> Update(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        await _contestantRepository.Update(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Contestant> Delete(Contestant entity)
    {
        GuardHelper.ThrowIfNull(_competition);

        await _contestantRepository.Delete(_competition.Id, entity);
        await _competitionRepository.Update(_competition);
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
        var competition = await _competitionRepository.Read(id);
        if (competition != null)
        {
            _competition = competition;
        }
    }
}
