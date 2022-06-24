using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Events;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager;

public class ManagerViewModel : ViewModelBase
{
    private static readonly DateTime Today = DateTime.Today;
    private readonly IEventAggregator eventAggregator;
    private readonly IExecutor<ManagerRoot> managerExecutor;
    private readonly IQueries<Participation> participations;

    public ManagerViewModel(
        IEventAggregator eventAggregator,
        IPopupService popupService,
        IExecutor<ManagerRoot> managerExecutor,
        IQueries<Participation> participations)
    {
        this.eventAggregator = eventAggregator;
        this.managerExecutor = managerExecutor;
        this.participations = participations;
        this.Update = new DelegateCommand(this.UpdateAction);
        this.Start = new DelegateCommand(this.StartAction);
        this.Disqualify = new DelegateCommand(this.DisqualifyAction);
        this.FailToQualify = new DelegateCommand(this.FailToQualifyAction);
        this.Resign = new DelegateCommand(this.ResignAction);
        this.ReInspection = new DelegateCommand(this.ReInspectionAction);
        this.RequireInspection = new DelegateCommand(this.RequireInspectionAction);
        this.StartList = new DelegateCommand(popupService.RenderStartList);
        this.Select = new DelegateCommand<object[]>(list =>
        {
            var participation = list.FirstOrDefault();
            if (participation != null)
            {
                this.SelectBy(participation as ParticipationGridModel);
            }
        });
    }

    public DelegateCommand<object[]> Select { get; }
    public DelegateCommand Start { get; }
    public DelegateCommand Update { get; }
    public DelegateCommand Disqualify { get; }
    public DelegateCommand FailToQualify { get; }
    public DelegateCommand Resign { get; }
    public DelegateCommand ReInspection { get; }
    public DelegateCommand RequireInspection { get; }
    public DelegateCommand StartList { get; }

    private Visibility startVisibility;
    private string inputNumber;
    private int? inputHours;
    private int? inputMinutes;
    private int? inputSeconds;
    private string notQualifiedReason;
    private bool requireInspectionValue = false;
    private bool reInspectionValue = false;

    public ObservableCollection<ParticipationGridModel> Participations { get; } = new();
    public ParticipationGridModel SelectedParticipation { get; set; }

    public override void OnNavigatedTo(NavigationContext context)
    {
        if (this.Participations.Any())
        {
            return;
        }
        var hasStarted = this.managerExecutor.Execute(x => x.HasStarted());
        if (hasStarted)
        {
            this.ReloadParticipations();
        }
    }

    private void StartAction()
    {
        this.managerExecutor.Execute(manager =>
        {
            manager.Start();
            this.ReloadParticipations();
        });
    }
    private void UpdateAction()
        => this.ExecuteAndRender((manager, number) => manager.UpdateRecord(number, this.InputTime));
    private void DisqualifyAction()
        => this.ExecuteAndRender((manager, number) => manager.Disqualify(number, this.NotQualifiedReason));
    private void FailToQualifyAction()
        => this.ExecuteAndRender((manager, number) => manager.FailToQualify(number, this.NotQualifiedReason));
    private void ResignAction()
        => this.ExecuteAndRender((manager, number) => manager.Resign(number, this.NotQualifiedReason));
    private void ReInspectionAction()
        => this.ExecuteAndRender((manager, number) => manager.ReInspection(number, this.ReInspectionValue));
    private void RequireInspectionAction()
        => this.ExecuteAndRender((manager, number) => manager.RequireInspection(number, this.RequireInspectionValue));
    private void ExecuteAndRender(Action<ManagerRoot, string> action)
    {
        if (string.IsNullOrWhiteSpace(this.InputNumber))
        {
            return;
        }
        var number = this.InputNumber;
        this.managerExecutor.Execute(manager =>
        {
            action(manager, number);
            this.ReloadParticipations();
        });
        this.SelectBy(number);
    }

    private void SelectBy(string number)
    {
        var participation = this.Participations.FirstOrDefault(x => x.Number == number);
        if (participation != null)
        {
            this.SelectBy(participation);
            this.eventAggregator.GetEvent<SelectTabEvent>().Publish(participation);
        }
    }

    private void SelectBy(ParticipationGridModel participation)
    {
        this.SelectedParticipation = participation;
        var performance = this.SelectedParticipation.Performances.LastOrDefault();
        if (performance != null)
        {
            this.ReInspectionValue = performance.IsReInspectionRequired;
            this.RequireInspectionValue = performance.IsRequiredInspectionRequired;
            this.NotQualifiedReason = participation.DisqualifyCode;
        }
        this.InputNumber = participation.Number;
    }

    private void ReloadParticipations()
    {
        this.Participations.Clear();
        this.StartVisibility = Visibility.Collapsed;
        var participations = this.participations.GetAll();
        if (participations.Any())
        {
            foreach (var participation in participations)
            {
                var viewModel = new ParticipationGridModel(participation);
                this.Participations.Add(viewModel);
            }
            this.SelectBy(this.Participations.First());
        }
    }

#region setters
    public Visibility StartVisibility
    {
        get => this.startVisibility;
        set => this.SetProperty(ref this.startVisibility, value);
    }
    public string InputNumber
    {
        get => this.inputNumber;
        set => this.SetProperty(ref this.inputNumber, value);
    }
    public string NotQualifiedReason
    {
        get => this.notQualifiedReason;
        set => this.SetProperty(ref this.notQualifiedReason, value);
    }
    public int? InputHours
    {
        get => this.inputHours;
        set => this.SetProperty(ref this.inputHours, value);
    }
    public int? InputMinutes
    {
        get => this.inputMinutes;
        set => this.SetProperty(ref this.inputMinutes, value);
    }
    public int? InputSeconds
    {
        get => this.inputSeconds;
        set => this.SetProperty(ref this.inputSeconds, value);
    }
    public bool ReInspectionValue
    {
        get => this.reInspectionValue;
        set => this.SetProperty(ref this.reInspectionValue, value);
    }
    public bool RequireInspectionValue
    {
        get => this.requireInspectionValue;
        set => this.SetProperty(ref this.requireInspectionValue, value);
    }
#endregion

    private DateTime InputTime
    {
        get
        {
            var time = Today;
            if (this.InputHours.HasValue)
            {
                time = time.AddHours(this.InputHours.Value);
            }
            if (this.InputMinutes.HasValue)
            {
                time = time.AddMinutes(this.InputMinutes.Value);
            }
            if (this.InputSeconds.HasValue)
            {
                time = time.AddSeconds(this.InputSeconds.Value);
            }
            return time;
        }
    }
}
