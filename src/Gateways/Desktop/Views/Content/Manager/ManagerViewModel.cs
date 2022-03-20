using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Events;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participations;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager;

public class ManagerViewModel : ViewModelBase
{
    private static readonly DateTime Today = DateTime.Today;
    private readonly IEventAggregator eventAggregator;
    private readonly IPopupService popupService;
    private readonly IExecutor<ManagerRoot> managerExecutor;
    private readonly IQueries<Participant> participants;

    public ManagerViewModel(
        IEventAggregator eventAggregator,
        IPopupService popupService,
        IExecutor<ManagerRoot> managerExecutor,
        IQueries<Participant> participants)
    {
        this.eventAggregator = eventAggregator;
        this.popupService = popupService;
        this.managerExecutor = managerExecutor;
        this.participants = participants;
        this.Update = new DelegateCommand(this.UpdateAction);
        this.Start = new DelegateCommand(this.StartAction);
        this.CompleteUnsuccessful = new DelegateCommand(this.CompleteUnsuccessfulAction);
        this.ReInspection = new DelegateCommand(this.ReInspectionAction);
        this.RequireInspection = new DelegateCommand(this.RequireInspectionAction);
        this.StartList = new DelegateCommand(popupService.RenderStartList);
    }

    public DelegateCommand Start { get; }
    public DelegateCommand Update { get; }
    public DelegateCommand CompleteUnsuccessful { get; }
    public DelegateCommand ReInspection { get; }
    public DelegateCommand RequireInspection { get; }
    public DelegateCommand StartList { get; }

    private Visibility startVisibility;
    private int? inputNumber;
    private int? inputHours;
    private int? inputMinutes;
    private int? inputSeconds;
    private string deQualificationCode;
    private bool requireInspectionValue = false;
    private bool reInspectionValue = false;

    public ObservableCollection<ParticipationTemplateModel> Participations { get; } = new();
    public ParticipationTemplateModel SelectedParticipation { get; set; }

    public override void OnNavigatedTo(NavigationContext context)
    {
        if (this.Participations.Any())
        {
            return;
        }
        var hasStarted = this.managerExecutor.Execute(x => x.HasStarted());
        if (hasStarted)
        {
            this.LoadParticipations();
        }
    }

    private void StartAction()
    {
        this.managerExecutor.Execute(manager =>
        {
            manager.Start();
            this.LoadParticipations();
        });
    }
    private void UpdateAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.SelectParticipation(this.InputNumber.Value);
        this.managerExecutor.Execute(manager =>
        {
            var performance = manager.UpdateRecord(this.InputNumber!.Value, this.InputTime);
            this.RenderPerformanceUpdate(performance);
        });
    }
    private void CompleteUnsuccessfulAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.SelectParticipation(this.InputNumber.Value);
        this.managerExecutor.Execute(manager =>
        {
            var performance = manager.CompletePerformance(this.InputNumber!.Value, this.DeQualificationCode);
            this.RenderPerformanceUpdate(performance);
        });
    }
    private void RenderPerformanceUpdate(Performance performance)
    {
        var existing = this.Participations
            .SelectMany(part => part.Performances)
            .FirstOrDefault(perf => perf.Id == performance.Id);
        if (existing == null)
        {
            var template = new PerformanceTemplateModel(performance);
            this.SelectedParticipation.Performances.Add(template);
        }
        else
        {
            existing.Update(performance);
        }
    }
    private void ReInspectionAction() // TODO extract common
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.SelectParticipation(this.InputNumber.Value);
        this.managerExecutor.Execute(manager =>
        {
            var opposite = !this.ReInspectionValue;
            manager.ReInspection(this.InputNumber!.Value, opposite);
            this.ReInspectionValue = opposite;
        });
    }
    private void RequireInspectionAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.SelectParticipation(this.InputNumber.Value);
        this.managerExecutor.Execute(manager =>
        {
            var opposite = !this.RequireInspectionValue;
            manager.RequireInspection(this.InputNumber!.Value, opposite);
            this.RequireInspectionValue = opposite;
        });
    }

    private void SelectParticipation(int number)
    {
        var participation = this.Participations.FirstOrDefault(x => x.Number == number);
        if (participation == null)
        {
            var message = string.Format(NOT_FOUND_MESSAGE, NUMBER, number);
            this.popupService.RenderValidation(message);
            return;
        }
        this.eventAggregator.GetEvent<SelectTabEvent>().Publish(participation);
        this.SelectedParticipation = participation;
        var performance = this.SelectedParticipation.Performances.LastOrDefault();
        if (performance != null)
        {
            this.ReInspectionValue = performance.IsReInspectionRequired;
            this.RequireInspectionValue = performance.IsRequiredInspectionRequired;
        }
    }

    private void LoadParticipations()
    {
        this.StartVisibility = Visibility.Collapsed;
        var participations = this.participants.GetAll();
        foreach (var participation in participations)
        {
            var performances = this.managerExecutor
                .Execute(x => x.GetPerformances(participation.Number))
                .ToList();
            var viewModel = new ParticipationTemplateModel(performances);
            this.Participations.Add(viewModel);
        }
    }

    #region setters
    public Visibility StartVisibility
    {
        get => this.startVisibility;
        set => this.SetProperty(ref this.startVisibility, value);
    }
    public int? InputNumber
    {
        get => this.inputNumber;
        set => this.SetProperty(ref this.inputNumber, value);
    }
    public string DeQualificationCode
    {
        get => this.deQualificationCode;
        set => this.SetProperty(ref this.deQualificationCode, value);
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
