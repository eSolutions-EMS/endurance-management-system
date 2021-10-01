using EnduranceJudge.Application.Actions.Manager.Queries.Participations;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations.Listing
{
    public class ParticipationListViewModel
        : SearchableListViewModelBase<GetParticipationList, UnusedCommend, ParticipationView>
    {
        public ParticipationListViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }
    }

    public class UnusedCommend : IdentifiableRequest
    {
    }
}
