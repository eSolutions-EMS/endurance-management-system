using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Services;
using MediatR;
using Prism.Commands;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class SearchableListViewModelBase<TListQuery, TRemoveCommand, TView>
        : ListViewModelBase<TListQuery, TRemoveCommand, TView>
        where TListQuery : IRequest<IEnumerable<ListItemModel>>, new()
        where TRemoveCommand : IdentifiableRequest, new()
        where TView : IView
    {
        protected SearchableListViewModelBase(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
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
            this.SearchValue = string.Empty;
        }
        private void ClearSearchAction()
        {
            this.SearchableItems.ClearSearch();
            this.SearchValue = string.Empty;
        }
    }
}
