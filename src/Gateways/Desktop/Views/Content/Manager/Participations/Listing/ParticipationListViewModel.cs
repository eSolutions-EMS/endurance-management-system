using EnduranceJudge.Application.Actions.Manager.Queries.GetParticipationList;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Regions;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations.Listing
{
    public class ParticipationListViewModel : SearchableListViewModelBase<GetParticipationList, ParticipationView>
    {
        public ParticipationListViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
        }
    }
}
