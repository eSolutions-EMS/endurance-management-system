using EnduranceJudge.Application.Services;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels;

public abstract class SearchableListViewModelBase<TView> : ListViewModelBase<TView>
    where TView : IView
{
    protected SearchableListViewModelBase(
        INavigationService navigation,
        IPersistence persistence,
        IPopupService service) : base(navigation, persistence, service)
    {
        Func<ListItemViewModel, string, bool> filter
            = (item, value) => item.Name.ToLower().Contains(value.ToLower());
        this.ListItems = new SearchableCollection<ListItemViewModel>(filter);
        this.Search = new DelegateCommand(this.SearchAction);
        this.ClearSearch = new DelegateCommand(this.ClearSearchAction);
    }

    public DelegateCommand Search { get; }
    public DelegateCommand ClearSearch { get; }

    private string searchValue;

    public string SearchValue
    {
        get => this.searchValue;
        set => this.SetProperty(ref this.searchValue, value);
    }
    private SearchableCollection<ListItemViewModel> SearchableItems
        => (SearchableCollection<ListItemViewModel>)this.ListItems;
    private void SearchAction()
    {
        this.SearchableItems.Search(this.searchValue);
    }
    private void ClearSearchAction()
    {
        this.SearchableItems.ClearSearch();
        this.SearchValue = string.Empty;
    }
}
