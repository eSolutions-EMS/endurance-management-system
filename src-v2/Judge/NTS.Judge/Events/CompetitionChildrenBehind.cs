using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using NTS.Domain.Setup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Events;
public class CompetitionChildrenBehind : INotBehindParent<Contestant>
{
    private readonly IRepository<Competition> _competitionRepository;
    private readonly IParentRepository<Contestant> _contestantRepository;
    private Competition _competition;

    public CompetitionChildrenBehind(IParentRepository<Contestant> contestants)
    {
        _contestantRepository = contestants;
    }
    IEnumerable<Contestant> INotBehindParent<Contestant>.Children => _competition?.Contestants ?? Enumerable.Empty<Contestant>();

    public async Task<Contestant> Create(Contestant entity)
    {
        await _contestantRepository.Create(_competition.Id, entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Contestant> Update(Contestant entity)
    {
        await _contestantRepository.Update(entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }

    public async Task<Contestant> Delete(Contestant entity)
    {
        await _contestantRepository.Delete(_competition.Id, entity);
        await _competitionRepository.Update(_competition);
        return entity;
    }


}
