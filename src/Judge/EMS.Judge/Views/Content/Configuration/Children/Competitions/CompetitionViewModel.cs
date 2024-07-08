using EMS.Judge.Common;
using EMS.Judge.Common.Components.Templates.ListItem;
using EMS.Judge.Common.Components.Templates.SimpleListItem;
using EMS.Judge.Services;
using EMS.Judge.Views.Content.Configuration.Children.Competitions.AddParticipants;
using EMS.Judge.Views.Content.Configuration.Children.Laps;
using EMS.Judge.Views.Content.Configuration.Core;
using EMS.Judge.Application.Common;
using Core.Domain.AggregateRoots.Configuration;
using Core.Domain.Common.Models;
using Core.Domain.State.Competitions;
using Core.Domain.Enums;
using Core.Domain.State.Participations;
using static Core.Localization.Strings;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
    private string _feiCategoryEventNumber;
    private string _feiScheduleNumber;
    private string _rule;
    private string _code;
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

    public string FeiCategoryEventNumber
    {
        get => _feiCategoryEventNumber;
        set => SetProperty(ref _feiCategoryEventNumber, value);
    }

    public string FeiScheduleNumber
    {
        get => _feiScheduleNumber;
        set => SetProperty(ref _feiScheduleNumber, value);
    }

    public string Rule
    {
        get => _rule;
        set => SetProperty(ref _rule, value);
    }

    public string EventCode
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }
    #endregion
}
