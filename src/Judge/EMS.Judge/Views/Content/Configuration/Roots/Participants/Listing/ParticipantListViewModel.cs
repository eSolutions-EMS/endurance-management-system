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
using Prism.Commands;
using EMS.Judge.Application.Hardware;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EMS.Judge.Views.Content.Configuration.Roots.Participants.Listing;

public class ParticipantListViewModel : SearchableListViewModelBase<ParticipantView>
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private readonly IQueries<Participant> participants;
    private VD67Controller vD67Controller;

    public ParticipantListViewModel(
        IPopupService popupService,
        IExecutor<ConfigurationRoot> executor,
        IQueries<Participant> participants,
        IPersistence persistence,
        INavigationService navigation) : base(navigation, persistence, popupService)
    {
        this.vD67Controller = new();
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
        model.AdditionalName = "Add (0 Tags)";
        model.AdditionalAction = new DelegateCommand(() => this.WriteAction(model));
        return model;
    }

    private void WriteAction(ListItemViewModel model)
    {
        var participant = this.participants.GetOne(model.Id);
        Task.Run(async () =>
        {
            var tag = await this.vD67Controller.Write(participant.Number);
            if (!participant.TagIds.Contains(tag))
            {
                participant.TagIds.Add(tag);
            }
            App.Current.Dispatcher.Invoke(() =>
            {
                model.AdditionalName = $"Add ({participant.TagIds.Count} Tags)";
            });
        });
    }

    protected override void RemoveDomain(int id)
        => this.executor.Execute(
            config => config.Participants.Remove(id),
            true);
}
