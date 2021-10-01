using EnduranceJudge.Application.Actions.Manager.Commands.UpdateParticipation;
using EnduranceJudge.Application.Actions.Manager.Queries.Participations;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager.ParticipationsInPhases;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static EnduranceJudge.Localization.Strings.Desktop;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations
{
    public class ParticipationViewModel : ViewModelBase
    {
        private readonly IApplicationService application;
        private readonly IDomainHandler domainHandler;
        // private Participation participation;

        public ParticipationViewModel(IApplicationService application, IDomainHandler domainHandler)
        {
            this.application = application;
            this.domainHandler = domainHandler;
            this.Update = new DelegateCommand(this.UpdateAction);
        }

        public DelegateCommand Update { get; }

        private int number;
        private DateTime inputTime = DateTime.Today;

        public ObservableCollection<ParticipationInPhaseViewModel> ParticipationsInPhases { get; } = new();

        private void UpdateAction()
        {
            var query = new GetParticipation
            {
                Number = this.Number,
            };

            var participation = this.application.Execute(query).Result;
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

            this.Act(participation, x => x.UpdateProgress(this.inputTime));
        }


        private async Task Act(Participation participation, Action<Participation> update)
        {
            this.domainHandler.Handle(() => update(participation));

            var command = new UpdateParticipation(participation);
            await this.application.Execute(command);

            this.MapFrom(participation);
        }

        public int Number
        {
            get => this.number;
            set => this.SetProperty(ref this.number, value);
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
