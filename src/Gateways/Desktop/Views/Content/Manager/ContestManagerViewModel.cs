using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participants;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager;

public class ContestManagerViewModel : ViewModelBase
{
    private static readonly DateTime Today = DateTime.Today;
    private readonly IExecutor<ManagerRoot> managerExecutor;
    private readonly IQueries<Participant> participants;

    public ContestManagerViewModel(
        IPopupService popupService,
        IExecutor<ManagerRoot> managerExecutor,
        IQueries<Participant> participants)
    {
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

    public ObservableCollection<ParticipantTemplateModel> Participants { get; } = new();

    public override void OnNavigatedTo(NavigationContext context)
    {
        if (this.Participants.Any())
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
        this.StartVisibility = Visibility.Collapsed;
    }
    private void UpdateAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.managerExecutor.Execute(manager =>
        {
            manager.UpdateRecord(this.InputNumber.Value, this.InputTime);
        });
        this.SelectParticipation();
    }
    private void CompleteUnsuccessfulAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.managerExecutor.Execute(manager =>
        {
            manager.CompletePerformance(this.InputNumber.Value, this.DeQualificationCode);
            this.SelectParticipation();
        });
    }
    private void ReInspectionAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        Action<ManagerRoot, bool> action = (manager, boolValue) => manager.ReInspection(
            this.InputNumber.Value,
            boolValue);
        Action<bool> setter = value => this.ReInspectionValue = value;
        this.CheckboxHandler(this.ReInspectionValue, action, setter);
    }
    private void RequireInspectionAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        Action<ManagerRoot, bool> action = (manager, boolValue) => manager.RequireInspection(
            this.InputNumber.Value,
            boolValue);
        Action<bool> setter = value => this.RequireInspectionValue = value;
        this.CheckboxHandler(this.RequireInspectionValue, action, setter);
    }
    private void CheckboxHandler(bool newValue, Action<ManagerRoot, bool> action, Action<bool> checkboxSetter)
    {
        var previousValue = !newValue;
        var isSuccessful = this.managerExecutor.Execute(manager =>
        {
            action(manager, newValue);
        });
        if (!isSuccessful)
        {
            checkboxSetter(previousValue);
        }
        this.SelectParticipation();
    }

    private void SelectParticipation()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }

        var participant = this.participants.GetOne(x => x.Number == this.InputNumber);
        if (participant == null)
        {
            return;
        }
        var participationViewModel = new ParticipantTemplateModel(participant, this.SelectParticipant, true);
        foreach (var participation in this.Participants)
        {
            participation.Visibility = Visibility.Collapsed;
        }
        var existing = this.Participants.FirstOrDefault(x => x.Number == this.InputNumber);
        if (existing != null)
        {
            this.Participants.Remove(existing);
        }
        this.Participants.Insert(0, participationViewModel);
        this.UpdateAdditionalInspectionCheckboxes(participant);
    }
    private void UpdateAdditionalInspectionCheckboxes(Participant participant)
    {
        var currentRecord = participant.LapRecords.Last();
        this.ReInspectionValue = currentRecord.IsReInspectionRequired;
        this.RequireInspectionValue = currentRecord.IsRequiredInspectionRequired;
    }

    private void LoadParticipations()
    {
        var participations = this.participants.GetAll();
        foreach (var participation in participations)
        {
            var viewModel = new ParticipantTemplateModel(participation, this.SelectParticipant);
            this.Participants.Add(viewModel);
        }
    }
    private void SelectParticipant(int number)
    {
        this.InputNumber = number;
        this.SelectParticipation();
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
