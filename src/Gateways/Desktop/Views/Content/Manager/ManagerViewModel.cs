using EnduranceJudge.Application.Actions.Manager.Commands;
using EnduranceJudge.Application.Actions.Manager.Commands.UpdateParticipation;
using EnduranceJudge.Application.Actions.Manager.Queries.Participations;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static EnduranceJudge.Localization.Strings.Desktop;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager
{
    public class ManagerViewModel : ViewModelBase
    {
        private readonly IApplicationService application;
        private readonly IDomainHandler domainHandler;

        public ManagerViewModel(IApplicationService application, IDomainHandler domainHandler)
        {
            this.application = application;
            this.domainHandler = domainHandler;
            this.Update = new DelegateCommand(this.UpdateAction);
            this.Start = new DelegateCommand(this.StartAction);
        }

        public DelegateCommand Start { get; }
        public DelegateCommand Update { get; }

        private int inputNumber;
        private DateTime inputTime = DateTime.Today;

        public ObservableCollection<ParticipationViewModel> Participations { get; } = new();

        private void UpdateAction()
        {
            this.CollapseParticipations();
            var participation = this.GetParticipation();
            if (participation == null)
            {
                this.ValidationError(PARTICIPANT_NOT_FOUND);
                return;
            }
            if (participation.IsComplete)
            {
                this.ValidationError(PARTICIPANT_HAS_COMPLETED_THE_PHASE);
                return;
            }

            this.Act(participation, x => x.UpdateProgress(this.InputTime));
        }

        private void StartAction()
        {
            var command = new StartParticipations();
            this.application.Execute(command);
        }

        private void CollapseParticipations()
        {
            foreach (var participation in this.Participations)
            {
                participation.Visibility = Visibility.Collapsed;
            }
        }

        private Participation GetParticipation()
        {
            var viewItem = this.Participations.FirstOrDefault(x => x.Number == this.InputNumber);
            Participation participation;
            if (viewItem != null)
            {
                this.Participations.Remove(viewItem);
                participation = viewItem.Participation;
            }
            else
            {
                var query = new GetParticipation
                {
                    Number = this.InputNumber,
                };
                participation = this.application.Execute(query).Result;
            }

            return participation;
        }

        private async Task Act(Participation participation, Action<Participation> update)
        {
            this.domainHandler.Handle(() => update(participation));

            var command = new UpdateParticipation(participation);
            await this.application.Execute(command);

            var viewModel = new ParticipationViewModel(this.InputNumber, participation);
            this.Participations.Add(viewModel);
        }

        public int InputNumber
        {
            get => this.inputNumber;
            set => this.SetProperty(ref this.inputNumber, value);
        }
        public DateTime InputTime
        {
            get => this.inputTime;
            set
            {
                var timeOfDay = value.TimeOfDay;
                var now = this.inputTime.Date.Add(timeOfDay);
                this.SetProperty(ref this.inputTime, now);
            }
        }
    }
}
