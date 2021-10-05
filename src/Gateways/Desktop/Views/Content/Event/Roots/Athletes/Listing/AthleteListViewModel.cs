using EnduranceJudge.Application.Models;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Events.Athletes;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Events;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes.Listing
{
    public class AthleteListViewModel : SearchableListViewModelBase<AthleteView>
    {
        private readonly IEventAggregator eventAggregator;
        public AthleteListViewModel(INavigationService navigation, IEventAggregator eventAggregator) : base(navigation)
        {
            this.eventAggregator = eventAggregator;
        }

        protected override IEnumerable<ListItemModel> LoadData() => throw new System.NotImplementedException();
        protected override void RemoveAction(int? id)
        {
            base.RemoveAction(id);
            this.eventAggregator
                .GetEvent<AthleteRemovedEvent>()
                .Publish(id!.Value);
        }
    }
}
