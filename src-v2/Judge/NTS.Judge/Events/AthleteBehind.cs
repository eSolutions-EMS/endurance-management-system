using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Safe;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Pages.Setup.Athletes;

namespace NTS.Judge.Events;

public class AthleteBehind : ObservableBehind<Athlete>,
    IListBehind<Athlete>,
    ICreateBehind<AthleteFormModel>,
    IUpdateBehind<AthleteFormModel>
{
    readonly IRepository<Athlete> _athleteRepository;

    public IReadOnlyList<Athlete> Items => ObservableCollection;

    public AthleteBehind(IRepository<Athlete> athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    async Task<AthleteFormModel> SafeCreate(AthleteFormModel entity)
    {
        var athlete = Athlete.Create(entity.Name, entity.FeiId, entity.Country, entity.Club, entity.Category);
        await _athleteRepository.Create(athlete);
        ObservableCollection.AddOrReplace(athlete);
        return entity;
    }

    async Task<AthleteFormModel> SafeUpdate(AthleteFormModel entity)
    {
        var athlete = Athlete.Update(entity.Id, entity.Name, entity.FeiId, entity.Country, entity.Club, entity.Category);
        await _athleteRepository.Update(athlete);
        ObservableCollection.AddOrReplace(athlete);
        return entity;
    }

    async Task<Athlete> SafeDelete(Athlete entity)
    {
        await _athleteRepository.Delete(x => x.Id == entity.Id);
        ObservableCollection.Remove(entity.Id);
        return entity;
    }

    #region SafePattern

    public async Task<AthleteFormModel> Create(AthleteFormModel athlete)
    {
        return await SafeHelper.Run(() => SafeCreate(athlete)) ?? athlete;
    }

    public async Task<AthleteFormModel> Update(AthleteFormModel athlete)
    {
        return await SafeHelper.Run(() => SafeUpdate(athlete)) ?? athlete;
    }

    public async Task<Athlete> Delete(Athlete athlete)
    {
        return await SafeHelper.Run(() => SafeDelete(athlete)) ?? athlete;
    }

    protected override async Task<bool> PerformInitialization()
    {
        var athletes = await _athleteRepository.ReadAll();
        ObservableCollection.AddRange(athletes);
        return athletes.Any();
    }

    #endregion
}
