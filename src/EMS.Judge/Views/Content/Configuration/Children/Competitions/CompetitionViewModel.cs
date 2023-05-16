using EMS.Judge.Core;
using EMS.Judge.Core.Components.Templates.ListItem;
using EMS.Judge.Core.Components.Templates.SimpleListItem;
using EMS.Judge.Services;
using EMS.Judge.Views.Content.Configuration.Children.Competitions.AddParticipants;
using EMS.Judge.Views.Content.Configuration.Children.Laps;
using EMS.Judge.Views.Content.Configuration.Core;
using EMS.Judge.Application.Core;
using EMS.Core.Domain.AggregateRoots.Configuration;
using EMS.Core.Domain.Core.Models;
using EMS.Core.Domain.State.Competitions;
using EMS.Core.Domain.Enums;
using EMS.Core.Domain.State.Participations;
using static EMS.Core.Localization.Strings;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static EMS.Judge.DesktopConstants;

namespace EMS.Judge.Views.Content.Configuration.Children.Competitions;

public class CompetitionViewModel : NestedConfigurationBase<CompetitionView, Competition>,
    ICompetitionState,
    ICollapsable
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private readonly IQueries<Participation> participations;
    public CompetitionViewModel() : this(null, null, null) { }
    public CompetitionViewModel(
        IExecutor<ConfigurationRoot> executor,
        IQueries<Participation> participations,
        IQueries<Competition> competitions) : base(competitions)
    {
        this.executor = executor;
        this.participations = participations;
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
        var result = this.executor.Execute(
            config => config.Competitions.Save(this),
            true);
        return result;
    }

    private void RemoveParticipantAction(int? participation)
    {
        this.executor.Execute(
            x => x.Competitions.RemoveParticipation(this.Id, participation!.Value),
            true);
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
        var parameter = new NavigationParameter(DesktopConstants.NavigationParametersKeys.DATA, tuple);
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
