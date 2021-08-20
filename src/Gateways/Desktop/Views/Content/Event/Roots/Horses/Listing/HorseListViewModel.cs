using EnduranceJudge.Application.Events.Queries.GetHorseList;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses.Listing
{
    public class HorseListViewModel : ListViewModelBase<GetHorseList, HorseView>
    {
        public HorseListViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }
    }
}
