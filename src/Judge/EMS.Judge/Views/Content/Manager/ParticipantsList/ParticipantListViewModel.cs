using EMS.Judge.Common.Services;
using EMS.Judge.Common.ViewModels;
using EMS.Judge.Services;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.Common;
using EMS.Judge.Application.Common.Models;
using Core.Mappings;
using Core.Domain.State.Participants;
using System.Collections.Generic;

namespace EMS.Judge.Views.Content.Manager.ParticipantsList;

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
