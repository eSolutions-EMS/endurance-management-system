using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Services;
using MediatR;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ListViewModelBase<TListQuery, TRemoveCommand, TView> : ViewModelBase
        where TListQuery : IRequest<IEnumerable<ListItemModel>>, new()
        where TRemoveCommand : IdentifiableRequest,  new()
        where TView : IView
    {
        protected ListViewModelBase(IApplicationService application, INavigationService navigation)
        {
            this.Navigation = navigation;
            this.ChangeToCreate = new DelegateCommand(this.ChangeToCreateAction);
            this.Application = application;
        }

        protected INavigationService Navigation { get; }
        protected IApplicationService Application { get; }

        public ObservableCollection<ListItemViewModel> ListItems { get; protected init; }
            = new (Enumerable.Empty<ListItemViewModel>());

        public DelegateCommand ChangeToCreate { get; }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext context)
        {
            base.OnNavigatedTo(context);

            this.LoadEvents();
        }

        private async Task LoadEvents()
        {
            var getEventsList = new TListQuery();
            var eventsList = await this.Application.Execute(getEventsList);

            var viewModels = eventsList
                .Select(this.ToViewModel)
                .ToList();

            this.ListItems.Clear();
            this.ListItems.AddRange(viewModels);
        }

        private ListItemViewModel ToViewModel(ListItemModel listable)
        {
            var update = new DelegateCommand<int?>(this.ChangeToUpdateAction);
            var remove = new DelegateCommand<int?>(this.RemoveAction);
            return new ListItemViewModel(listable.Id, listable.Name, update, remove);
        }

        protected virtual void ChangeToCreateAction()
        {
            this.Navigation.ChangeTo<TView>();
        }

        protected virtual void ChangeToUpdateAction(int? id)
        {
            this.Navigation.ChangeTo<TView>(id!.Value);
        }

        protected virtual void RemoveAction(int? id)
        {
            var remove = new TRemoveCommand
            {
                Id = id!.Value,
            };
            this.Application.Execute(remove);
            var item = this.ListItems.FirstOrDefault(i => i.Id == id!.Value);
            this.ListItems.Remove(item);
        }
    }
}
