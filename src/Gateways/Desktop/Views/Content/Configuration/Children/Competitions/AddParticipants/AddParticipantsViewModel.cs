using EnduranceJudge.Application.Services;
using EnduranceJudge.Application.Core;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using static EnduranceJudge.Localization.Strings;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions.AddParticipants;

public class AddParticipantsViewModel : SearchableListViewModelBase<AddParticipantsView>
{
    private readonly IExecutor<ConfigurationRoot> configurationExecutor;
    private readonly IQueries<Participant> participants;
    private readonly IQueries<Participation> participations;
    private int competitionId;

    public AddParticipantsViewModel(
        IExecutor<ConfigurationRoot> configurationExecutor,
        IQueries<Participant> participants,
        IQueries<Participation> participations,
        INavigationService navigation,
        IPersistence persistence,
        IPopupService service) : base(navigation, persistence, service)
    {
        this.AllowCreate = false;
        this.configurationExecutor = configurationExecutor;
        this.participants = participants;
        this.participations = participations;
    }

    public override void OnNavigatedTo(NavigationContext context)
    {
        var (id, name) = (ValueTuple<int, string>)context.GetData();
        this.competitionId = id;
        this.CompetitionName = name;
        base.OnNavigatedTo(context);
    }

    private string competitionName;

    protected override IEnumerable<ListItemModel> LoadData()
    {
        var participationIds = this.participations
            .GetAll()
            .Where(x => x.CompetitionsIds.Contains(this.competitionId))
            .Select(y => y.Participant.Id);
        var participants = this.participants
            .GetAll()
            .Where(x => !participationIds.Contains(x.Id))
            .MapEnumerable<ListItemModel>();
        return participants;
    }

    protected override ListItemViewModel ToViewModel(ListItemModel listable)
    {
        var addAction = new DelegateCommand<int?>(this.AddParticipantAction);
        var viewModel = new ListItemViewModel(listable.Id, listable.Name, addAction, ADD);
        return viewModel;
    }

    private void AddParticipantAction(int? participantId)
    {
        this.configurationExecutor.Execute(configuration =>
        {
            configuration.Participants.AddParticipation(this.competitionId, participantId!.Value);
            var listItem = this.ListItems.FirstOrDefault(x => x.Id == participantId!.Value);
            this.ListItems.Remove(listItem);
        }, true);
    }

    public string CompetitionName
    {
        get => this.competitionName;
        private set => this.SetProperty(ref this.competitionName, value);
    }

}
