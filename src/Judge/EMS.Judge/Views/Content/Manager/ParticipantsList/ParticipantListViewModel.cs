﻿using EMS.Judge.Core.Services;
using EMS.Judge.Core.ViewModels;
using EMS.Judge.Services;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.Core;
using EMS.Judge.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.State.Participants;
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