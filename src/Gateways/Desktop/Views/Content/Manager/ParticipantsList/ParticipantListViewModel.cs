using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.ParticipantsList;

public class ParticipantListViewModel : SearchableListViewModelBase<ManagerView>
{
    private readonly IQueries<Participant> participants;
    public ParticipantListViewModel(
        IPopupService popupService,
        IQueries<Participant> participants,
        IPersistence persistence,
        INavigationService navigation) : base(navigation, persistence, popupService)
    {
        this.AllowCreate = false;
        this.AllowDelete = false;
        this.participants = participants;
    }

    protected override IEnumerable<ListItemModel> LoadData()
    {
        var participants = this.participants.GetAll();
        var viewModels = participants.MapEnumerable<ListItemModel>();
        return viewModels;
    }
}
