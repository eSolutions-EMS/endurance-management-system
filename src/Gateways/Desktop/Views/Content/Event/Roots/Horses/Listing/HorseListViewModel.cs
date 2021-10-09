using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Events.Horses;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Events;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses.Listing
{
    public class HorseListViewModel : SearchableListViewModelBase<HorseView>
    {
        private readonly IEventAggregator eventAggregator;
        public HorseListViewModel(INavigationService navigation, IEventAggregator eventAggregator) : base(navigation)
        {
            this.eventAggregator = eventAggregator;
        }

        protected override IEnumerable<ListItemModel> LoadData() => throw new System.NotImplementedException();
        protected override void RemoveAction(int? id)
        {
            base.RemoveAction(id);
            this.eventAggregator
                .GetEvent<HorseRemovedEvent>()
                .Publish(id!.Value);
        }
    }
}
