using EnduranceJudge.Application.Services;
using EnduranceJudge.Application.Core;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Horses.Listing;

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
        => this.executor.Execute(config =>
            config.Horses.Remove(id));
}
