using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ListViewModelBase<TView> : ViewModelBase
        where TView : IView
    {
        protected ListViewModelBase(INavigationService navigation)
        {
            this.Navigation = navigation;
            this.ChangeToCreate = new DelegateCommand(this.ChangeToCreateAction);
        }

        protected INavigationService Navigation { get; }

        public ObservableCollection<ListItemViewModel> ListItems { get; protected init; }
            = new (Enumerable.Empty<ListItemViewModel>());

        public DelegateCommand ChangeToCreate { get; }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.Load();
        }

        protected abstract IEnumerable<ListItemModel> LoadData();

        protected virtual void ChangeToCreateAction()
        {
            this.Navigation.ChangeTo<TView>();
        }
        protected virtual void ChangeToUpdateAction(int? id)
        {
            this.Navigation.ChangeTo<TView>(id!.Value);
        }

        protected virtual void Load()
        {
            var eventsList = this.LoadData();

            var viewModels = eventsList
                .Select(this.ToViewModel)
                .ToList();

            this.ListItems.Clear();
            this.ListItems.AddRange(viewModels);
        }
        protected virtual void RemoveAction(int? id)
        {
            this.RemoveItem(id!.Value);
            var item = this.ListItems.FirstOrDefault(i => i.Id == id!.Value);
            this.ListItems.Remove(item);
        }
        protected virtual void RemoveItem(int id)
        {
            throw new NotImplementedException();
        }

        private ListItemViewModel ToViewModel(ListItemModel listable)
        {
            var update = new DelegateCommand<int?>(this.ChangeToUpdateAction);
            var remove = new DelegateCommand<int?>(this.RemoveAction);
            return new ListItemViewModel(listable.Id, listable.Name, update, remove);
        }
    }
}
