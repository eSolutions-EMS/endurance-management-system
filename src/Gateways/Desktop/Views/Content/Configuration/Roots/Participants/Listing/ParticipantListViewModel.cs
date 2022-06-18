using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Participants.Listing;

public class ParticipantListViewModel : SearchableListViewModelBase<ParticipantView>
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private readonly IQueries<Participant> participants;

    public ParticipantListViewModel(
        IPopupService popupService,
        IExecutor<ConfigurationRoot> executor,
        IQueries<Participant> participants,
        IPersistence persistence,
        INavigationService navigation) : base(navigation, persistence, popupService)
    {
        this.executor = executor;
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
        => this.executor.Execute(config =>
            config.Participants.Remove(id));
}
