using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Localization.Translations;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions.AddParticipants;

public class AddParticipantsViewModel : SearchableListViewModelBase<AddParticipantsView>
{
    private readonly IExecutor<ConfigurationRoot> configurationExecutor;
    private readonly IQueries<Participant> participants;
    private int competitionId;

    public AddParticipantsViewModel(
        IExecutor<ConfigurationRoot> configurationExecutor,
        IQueries<Participant> participants,
        INavigationService navigation,
        IPersistence persistence,
        IPopupService service) : base(navigation, persistence, service)
    {
        this.AllowCreate = false;
        this.configurationExecutor = configurationExecutor;
        this.participants = participants;
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
        var participants = this.participants
            .GetAll()
            .Where(x => x.Participation
                .Competitions
                .All(comp => comp.Id != this.competitionId))
            .MapEnumerable<ListItemModel>();
        return participants;
    }

    protected override ListItemViewModel ToViewModel(ListItemModel listable)
    {
        var addAction = new DelegateCommand<int?>(this.AddParticipantAction);
        var viewModel = new ListItemViewModel(listable.Id, listable.Name, addAction, Words.ADD);
        return viewModel;
    }

    private void AddParticipantAction(int? participantId)
    {
        this.configurationExecutor.Execute(x => x
            .Participants
            .AddParticipation(this.competitionId, participantId!.Value));
        var listItem = this.ListItems.FirstOrDefault(x => x.Id == participantId!.Value);
        this.ListItems.Remove(listItem);
    }

    public string CompetitionName
    {
        get => this.competitionName;
        private set => this.SetProperty(ref this.competitionName, value);
    }

}
