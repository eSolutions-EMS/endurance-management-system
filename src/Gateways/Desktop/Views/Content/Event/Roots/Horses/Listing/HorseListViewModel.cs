using EnduranceJudge.Application.Events.Commands.Horses;
using EnduranceJudge.Application.Events.Queries.GetHorseList;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Events;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Events;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses.Listing
{
    public class HorseListViewModel : SearchableListViewModelBase<GetHorseList, RemoveHorse, HorseView>
    {
        private readonly IEventAggregator eventAggregator;
        public HorseListViewModel(
            IApplicationService application,
            INavigationService navigation,
            IEventAggregator eventAggregator)
            : base(application, navigation)
        {
            this.eventAggregator = eventAggregator;
        }

        protected override void RemoveAction(int? id)
        {
            base.RemoveAction(id);
            this.eventAggregator
                .GetEvent<HorseRemovedEvent>()
                .Publish(id!.Value);
        }
    }
}
