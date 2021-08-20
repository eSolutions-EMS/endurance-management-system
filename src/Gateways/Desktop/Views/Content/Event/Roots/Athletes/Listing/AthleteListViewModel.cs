using EnduranceJudge.Application.Events.Queries.GetAthletesList;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes.Listing
{
    public class AthleteListViewModel : ListViewModelBase<GetAthletesList, AthleteView>
    {
        public AthleteListViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }
    }
}
