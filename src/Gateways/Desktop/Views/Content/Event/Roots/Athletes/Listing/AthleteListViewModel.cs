using EnduranceJudge.Application.Events.Commands.Athletes;
using EnduranceJudge.Application.Events.Queries.GetAthletesList;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Events;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Events;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes.Listing
{
    public class AthleteListViewModel : SearchableListViewModelBase<GetAthletesList, RemoveAthlete, AthleteView>
    {
        private readonly IEventAggregator eventAggregator;
        public AthleteListViewModel(
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
                .GetEvent<AthleteRemovedEvent>()
                .Publish(id!.Value);
        }
    }
}
