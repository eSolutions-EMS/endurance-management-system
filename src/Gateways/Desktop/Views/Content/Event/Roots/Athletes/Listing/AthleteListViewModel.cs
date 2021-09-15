using EnduranceJudge.Application.Events.Queries.GetAthletesList;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes.Listing
{
    public class AthleteListViewModel : SearchableListViewModelBase<GetAthletesList, AthleteView>
    {
        public AthleteListViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }
    }
}
