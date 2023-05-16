using EMS.Judge.Core.Services;
using EMS.Judge.Core.ViewModels;
using EMS.Judge.Services;
using EMS.Core.Application.Services;
using EMS.Core.Application.Core;
using EMS.Core.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.AggregateRoots.Configuration;
using EMS.Core.Domain.State.Participants;
using System.Collections.Generic;

namespace EMS.Judge.Views.Content.Configuration.Roots.Participants.Listing;

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
        => this.executor.Execute(
            config => config.Participants.Remove(id),
            true);
}
