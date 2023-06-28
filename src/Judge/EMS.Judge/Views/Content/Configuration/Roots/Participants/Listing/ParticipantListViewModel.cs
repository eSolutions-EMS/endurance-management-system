using EMS.Judge.Common.Services;
using EMS.Judge.Common.ViewModels;
using EMS.Judge.Services;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.Common;
using EMS.Judge.Application.Common.Models;
using Core.Mappings;
using Core.Domain.AggregateRoots.Configuration;
using Core.Domain.State.Participants;
using System.Collections.Generic;
using EMS.Judge.Common.Components.Templates.ListItem;

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

    protected override ListItemViewModel ToViewModel(ListItemModel listable)
    {
        var model = base.ToViewModel(listable);
        return model;
    }

    protected override void RemoveDomain(int id)
        => this.executor.Execute(
            config => config.Participants.Remove(id),
            true);
}
