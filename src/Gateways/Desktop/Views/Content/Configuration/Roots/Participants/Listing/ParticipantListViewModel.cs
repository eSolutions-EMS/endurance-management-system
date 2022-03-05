using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Participants.Listing
{
    public class ParticipantListViewModel : SearchableListViewModelBase<ParticipantView>
    {
        private readonly ConfigurationRoot aggregate;
        private readonly IQueries<Participant> participants;

        public ParticipantListViewModel(
            IPopupService popupService,
            ConfigurationRoot aggregate,
            IQueries<Participant> participants,
            IPersistence persistence,
            INavigationService navigation) : base(navigation, persistence, popupService)
        {
            this.aggregate = aggregate;
            this.participants = participants;
        }

        protected override IEnumerable<ListItemModel> LoadData()
        {
            var participants = this.participants
                .GetAll()
                .MapEnumerable<ListItemModel>();
            return participants;
        }

        protected override void RemoveDomain(int id)
        {
            this.aggregate.Participants.Remove(id);
        }
    }
}
