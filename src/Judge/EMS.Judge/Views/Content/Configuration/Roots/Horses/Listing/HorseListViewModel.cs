using EMS.Judge.Core.Services;
using EMS.Judge.Core.ViewModels;
using EMS.Judge.Services;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.Core;
using EMS.Judge.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.AggregateRoots.Configuration;
using EMS.Core.Domain.State.Horses;
using System.Collections.Generic;

namespace EMS.Judge.Views.Content.Configuration.Roots.Horses.Listing;

public class HorseListViewModel : SearchableListViewModelBase<HorseView>
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private readonly IQueries<Horse> horses;

    public HorseListViewModel(
        IPopupService popupService,
        IExecutor<ConfigurationRoot> executor,
        IQueries<Horse> horses,
        IPersistence persistence,
        INavigationService navigation) : base(navigation, persistence, popupService)
    {
        this.executor = executor;
        this.horses = horses;
    }

    protected override IEnumerable<ListItemModel> LoadData()
    {
        var horses = this.horses
            .GetAll()
            .MapEnumerable<ListItemModel>();
        return horses;
    }
    protected override void RemoveDomain(int id)
        => this.executor.Execute(
            config => config.Horses.Remove(id),
            true);
}
