using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Aggregates.Manager;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participants;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager
{
    public class ContestManagerViewModel : ViewModelBase
    {
        private static readonly DateTime Today = DateTime.Today;
        private readonly IExecutor<ContestManager> contestExecutor;
        private readonly IQueries<Participant> participants;

        public ContestManagerViewModel(IExecutor<ContestManager> contestExecutor, IQueries<Participant> participants)
        {
            this.contestExecutor = contestExecutor;
            this.participants = participants;
            this.Update = new DelegateCommand(this.UpdateAction);
            this.Start = new DelegateCommand(this.StartAction);
            this.Complete = new DelegateCommand(this.CompleteAction);
            this.CompleteUnsuccessful = new DelegateCommand(this.CompleteUnsuccessfulAction);
            this.RequireInspection = new DelegateCommand(this.RequireInspectionAction);
            this.CompleteInspection = new DelegateCommand(this.CompleteInspectionAction);
        }

        public DelegateCommand Start { get; }
        public DelegateCommand Update { get; }
        public DelegateCommand Complete { get; }
        public DelegateCommand CompleteUnsuccessful { get; }
        public DelegateCommand RequireInspection { get; }
        public DelegateCommand CompleteInspection { get; }

        private Visibility startVisibility;
        private int? inputNumber;
        private int? inputHours;
        private int? inputMinutes;
        private int? inputSeconds;
        private string deQualificationCode;
        private bool requireInspectionValue;

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
        private void CompleteAction()
        {
            if (!this.InputNumber.HasValue)
            {
                return;
            }
            this.contestExecutor.Execute(manager =>
            {
                manager.CompletePerformance(this.InputNumber.Value);
                this.LoadParticipation();
            });
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
        private void RequireInspectionAction()
        {
            if (!this.InputNumber.HasValue)
            {
                return;
            }
            this.contestExecutor.Execute(manager =>
            {
                manager.RequireInspection(this.InputNumber.Value);
                this.RequireInspectionValue = false;
            });
        }
        private void CompleteInspectionAction()
        {
            if (!this.InputNumber.HasValue)
            {
                return;
            }
            this.contestExecutor.Execute(manager =>
                manager.CompleteRequiredInspection(this.InputNumber.Value));
        }

        private void LoadParticipation()
        {
            if (!this.InputNumber.HasValue)
            {
                return;
            }

            var participant = this.participants.GetOne(x => x.Number == this.InputNumber);
            var participationViewModel = new ParticipantTemplateModel(participant);

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
        }

        private void LoadParticipants()
        {
            var participants = this.participants.GetAll();
            foreach (var participant in participants)
            {
                var viewModel = new ParticipantTemplateModel(participant, Visibility.Collapsed);
                this.Participants.Add(viewModel);
            }
        }

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
        public bool RequireInspectionValue
        {
            get => this.requireInspectionValue;
            set => this.SetProperty(ref this.requireInspectionValue, true);
        }

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
}
