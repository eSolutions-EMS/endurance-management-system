using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using MediatR;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ListViewModelBase<TApplicationCommand, TView> : ViewModelBase
        where TApplicationCommand : IRequest<IEnumerable<ListItemModel>>, new()
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

        public ObservableCollection<ListItemViewModel> ListItems { get; }
            = new (Enumerable.Empty<ListItemViewModel>());

        public DelegateCommand ChangeToCreate { get; }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            base.OnNavigatedTo(navigationContext);

            this.LoadEvents();
        }

        private async Task LoadEvents()
        {
            var getEventsList = new TApplicationCommand();
            var eventsList = await this.Application.Execute(getEventsList);

            var viewModels = eventsList
                .Select(this.ToViewModel)
                .ToList();

            this.ListItems.Clear();
            this.ListItems.AddRange(viewModels);
        }

        private ListItemViewModel ToViewModel(IListable listable)
        {
            var command = new DelegateCommand<int?>(this.ChangeToUpdateAction);
            return new ListItemViewModel(listable.Id, listable.Name, command);
        }

        protected virtual void ChangeToCreateAction()
        {
            this.Navigation.ChangeTo<TView>();
        }

        protected virtual void ChangeToUpdateAction(int? id)
        {
            this.Navigation.ChangeTo<TView>(id.Value);
        }
    }
}
