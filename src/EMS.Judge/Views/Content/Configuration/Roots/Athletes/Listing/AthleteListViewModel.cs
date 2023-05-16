using EMS.Judge.Core.Services;
using EMS.Judge.Core.ViewModels;
using EMS.Judge.Services;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.Core;
using EMS.Judge.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.AggregateRoots.Configuration;
using EMS.Core.Domain.State.Athletes;
using System.Collections.Generic;

namespace EMS.Judge.Views.Content.Configuration.Roots.Athletes.Listing;

public class AthleteListViewModel : SearchableListViewModelBase<AthleteView>
{
    private readonly IExecutor<ConfigurationRoot> configurationExecutor;
    private readonly IQueries<Athlete> athletes;
    public AthleteListViewModel(
        IPopupService popupService,
        IExecutor<ConfigurationRoot> configurationExecutor,
        IPersistence persistence,
        IQueries<Athlete> athletes,
        INavigationService navigation) : base(navigation, persistence, popupService)
    {
        this.configurationExecutor = configurationExecutor;
        this.athletes = athletes;
    }

    protected override IEnumerable<ListItemModel> LoadData()
    {
        var athletes = this.athletes
            .GetAll()
            .MapEnumerable<ListItemModel>();
        return athletes;
    }
    protected override void RemoveDomain(int id)
    {
        this.configurationExecutor.Execute(
            x => x.Athletes.Remove(id),
            true);
    }
}
