using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Exceptions;
using Not.Extensions;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Pages.Setup.Contestants;

namespace NTS.Judge.Events;

public class ContestantBehind : SimpleCrudBehind<Contestant, ContestantFormModel>
{
    private readonly IRepository<Competition> _competitions;

    public ContestantBehind(IRepository<Competition> competitions, IRepository<Contestant> participations) : base(participations)
    {
        _competitions = competitions;
    }

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        if (arguments.FirstOrDefault() is not int competitionId)
        {
            throw GuardHelper.Exception("Competition ID is required");
        }
        if (CompetitionChildrenBehind.Competition?.Id == competitionId)
        {
            return false;
        }

        var competition = await _competitions.Read(competitionId);
        GuardHelper.ThrowIfDefault(competition);

        CompetitionChildrenBehind.Competition = competition;
        ObservableCollection.AddRange(competition.Contestants);

        return false; // This allows us to change the parent competition
    }

    protected override async Task OnBeforeCreate(Contestant entity)
    {
        CompetitionChildrenBehind.Competition!.Add(entity);
        await _competitions.Update(CompetitionChildrenBehind.Competition);
    }

    protected override async Task OnBeforeUpdate(Contestant entity)
    {
        CompetitionChildrenBehind.Competition!.Update(entity);
        await _competitions.Update(CompetitionChildrenBehind.Competition);
    }

    protected override async Task OnBeforeDelete(Contestant entity)
    {
        CompetitionChildrenBehind.Competition!.Remove(entity);
        await _competitions.Update(CompetitionChildrenBehind.Competition);
    }

    protected override Contestant CreateEntity(ContestantFormModel model)
    {
        return Contestant.Create(model.StartTimeOverride?.ToDateTimeOffset(), model.IsUnranked, model.Combination);
    }

    protected override Contestant UpdateEntity(ContestantFormModel model)
    {
        return Contestant.Update(model.Id, model.StartTimeOverride?.ToDateTimeOffset(), model.IsUnranked, model.Combination);
    }
}
