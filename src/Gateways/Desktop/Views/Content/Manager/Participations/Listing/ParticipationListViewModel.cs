using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations.Listing
{
    public class ParticipationListViewModel : SearchableListViewModelBase<ManagerView>
    {
        public ParticipationListViewModel(INavigationService navigation) : base(navigation)
        {
        }

        protected override IEnumerable<ListItemModel> LoadData() => throw new System.NotImplementedException();
    }
}
