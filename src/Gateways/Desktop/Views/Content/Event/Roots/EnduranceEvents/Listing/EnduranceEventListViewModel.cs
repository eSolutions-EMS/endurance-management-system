using EnduranceJudge.Application.Events.Queries.GetEnduranceEventsList;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents.Listing
{
    public class EnduranceEventListViewModel : ListViewModelBase<GetEnduranceEventsList, EnduranceEventView>
    {
        public EnduranceEventListViewModel(IApplicationService application, INavigationService navigation)
            : base (application, navigation)
        {
        }

        public ObservableCollection<ListItemViewModel> EnduranceEvents => this.ListItems;
    }
}
