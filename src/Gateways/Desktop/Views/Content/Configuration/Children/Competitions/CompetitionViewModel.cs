using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions.AddParticipants;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Laps;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using static EnduranceJudge.Localization.Strings;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions;

public class CompetitionViewModel : NestedConfigurationBase<CompetitionView, Competition>,
    ICompetitionState,
    ICollapsable
{
    private readonly IExecutor<ConfigurationRoot> configurationExecutor;
    private readonly IQueries<Participation> participations;
    private readonly ConfigurationRoot configuration;
    public CompetitionViewModel() : this(null, null, null, null) { }
    public CompetitionViewModel(
        IExecutor<ConfigurationRoot> configurationExecutor,
        IQueries<Participation> participations,
        ConfigurationRoot configuration,
        IQueries<Competition> competitions) : base(competitions)
    {
        this.configurationExecutor = configurationExecutor;
        this.participations = participations;
        this.configuration = configuration;
        this.AddParticipants = new DelegateCommand(this.NavigateToAddParticipants);
        this.ToggleVisibility = new DelegateCommand(this.ToggleVisibilityAction);
        this.CreateLap = new DelegateCommand(this.NewForm<LapView>);
    }

    public DelegateCommand AddParticipants { get; }
    public DelegateCommand ToggleVisibility { get; }
    public DelegateCommand CreateLap { get; }
    public ObservableCollection<SimpleListItemViewModel> TypeItems { get; }
        = new(SimpleListItemViewModel.FromEnum<CompetitionType>());
    public ObservableCollection<LapViewModel> Laps { get; } = new();
    public ObservableCollection<ListItemViewModel> Participants { get; } = new();

    private int typeValue;
    private string name;
    private string typeString;
    private string toggleText = EXPAND;
    private DateTime startTime = DateTime.Today;
    private Visibility visibility = Visibility.Collapsed;
    public CompetitionType Type => (CompetitionType)this.TypeValue;

    public override void OnNavigatedTo(NavigationContext context)
    {
        base.OnNavigatedTo(context);
        this.LoadParticipations();
    }
    protected override IDomain Persist()
    {
        var result = this.configuration.Competitions.Save(this);
        return result;
    }

    private void RemoveParticipantAction(int? participation)
    {
        this.configurationExecutor.Execute(x =>
            x.Competitions.RemoveParticipation(this.Id, participation!.Value));
        this.LoadParticipations();
    }

    private void LoadParticipations()
    {
        this.Participants.Clear();
        var participations = this.participations
            .GetAll()
            .Where(x => x.CompetitionsIds.Any(id => id == this.Id));
        foreach (var participation in participations)
        {
            var removeCommand = new DelegateCommand<int?>(this.RemoveParticipantAction);
            var listItem = new ListItemViewModel(
                participation.Id, // ??
                participation.Participant.Name,
                removeCommand,
                REMOVE);
            this.Participants.Add(listItem);
        }
    }

    private void NavigateToAddParticipants()
    {
        var tuple = (this.Id, this.Name);
        var parameter = new NavigationParameter(NavigationParametersKeys.DATA, tuple);
        this.Navigation.ChangeTo<AddParticipantsView>(parameter);
    }

    private void ToggleVisibilityAction()
    {
        if (this.Visibility == Visibility.Collapsed)
        {
            this.Visibility = Visibility.Visible;
            this.ToggleText = COLLAPSE;
        }
        else
        {
            this.Visibility = Visibility.Collapsed;
            this.ToggleText = EXPAND;
        }
    }

    #region Setters
    public string TypeString
    {
        get => this.typeString;
        set => this.SetProperty(ref this.typeString, value);
    }
    public int TypeValue
    {
        get => this.typeValue;
        set
        {
            this.SetProperty(ref this.typeValue, value);
            this.TypeString = ((CompetitionType)this.typeValue).ToString();
        }
    }
    public string Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value);
    }
    public DateTime StartTime
    {
        get => this.startTime;
        set => this.SetProperty(ref this.startTime, value);
    }
    public string ToggleText
    {
        get => this.toggleText;
        set => this.SetProperty(ref this.toggleText, value);
    }
    public Visibility Visibility
    {
        get => this.visibility;
        private set => this.SetProperty(ref this.visibility, value);
    }
    #endregion
}
