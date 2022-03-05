using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Aggregates.Manager;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participants;
using Microsoft.AspNetCore.CookiePolicy;
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
    private readonly IPopupService popupService;
    private readonly IExecutor<ManagerRoot> contestExecutor;
    private readonly IQueries<Participant> participants;

    public ContestManagerViewModel(
        IPopupService popupService,
        IExecutor<ManagerRoot> contestExecutor,
        IQueries<Participant> participants)
    {
        this.popupService = popupService;
        this.contestExecutor = contestExecutor;
        this.participants = participants;
        this.Update = new DelegateCommand(this.UpdateAction);
        this.Start = new DelegateCommand(this.StartAction);
        this.CompleteUnsuccessful = new DelegateCommand(this.CompleteUnsuccessfulAction);
        this.ReInspection = new DelegateCommand(this.ReInspectionAction);
        this.RequireInspection = new DelegateCommand(this.RequireInspectionAction);
        this.StartList = new DelegateCommand(this.popupService.RenderStartList);
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
        var hasStarted = this.contestExecutor.Execute(x => x.HasStarted());
        if (hasStarted)
        {
            this.LoadParticipants();
        }
    }

    private void StartAction()
    {
        this.contestExecutor.Execute(manager =>
        {
            manager.Start();
            this.LoadParticipants();
        });
        this.StartVisibility = Visibility.Collapsed;
    }
    private void UpdateAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.contestExecutor.Execute(manager =>
        {
            manager.UpdatePerformance(this.InputNumber.Value, this.InputTime);
        });
        this.LoadParticipation();
    }
    private void CompleteUnsuccessfulAction()
    {
        if (!this.InputNumber.HasValue)
        {
            return;
        }
        this.contestExecutor.Execute(manager =>
        {
            manager.CompletePerformance(this.InputNumber.Value, this.DeQualificationCode);
            this.LoadParticipation();
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
        var isSuccessful = this.contestExecutor.Execute(manager =>
        {
            action(manager, newValue);
        });
        if (!isSuccessful)
        {
            checkboxSetter(previousValue);
        }
        this.LoadParticipation();
    }

    private void LoadParticipation()
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
        var currentPerformance = participant.Participation.Performances.Last();
        this.ReInspectionValue = currentPerformance.IsReInspectionRequired;
        this.RequireInspectionValue = currentPerformance.IsRequiredInspectionRequired;
    }

    private void LoadParticipants()
    {
        var participants = this.participants.GetAll();
        foreach (var participant in participants)
        {
            var viewModel = new ParticipantTemplateModel(participant, this.SelectParticipant);
            this.Participants.Add(viewModel);
        }
    }
    private void SelectParticipant(int number)
    {
        this.InputNumber = number;
        this.LoadParticipation();
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
