using EnduranceJudge.Application.Actions.Manager.Commands.UpdateParticipation;
using EnduranceJudge.Application.Actions.Manager.Queries.GetParticipation;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Commands;
using System;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations
{
    public class ParticipationViewModel
        : RootFormBase<GetParticipation, UpdateParticipation, Participation, ParticipationView>
    {
        private Participation participation;

        public ParticipationViewModel(IApplicationService application) : base(application)
        {
            this.Start = new DelegateCommand(this.StartAction);
            this.Arrive = new DelegateCommand(this.ArriveAction);
            this.Inspect = new DelegateCommand(this.InspectAction);
            this.ReInspect = new DelegateCommand(this.ReInspectAction);
            this.CompleteSuccessful = new DelegateCommand(this.CompleteSuccessfulAction);
            this.CompleteUnsuccessful = new DelegateCommand(this.CompleteUnsuccessfulAction);
        }

        public DelegateCommand Start { get; }
        public DelegateCommand Arrive { get; }
        public DelegateCommand Inspect { get; }
        public DelegateCommand ReInspect { get; }
        public DelegateCommand CompleteSuccessful { get; }
        public DelegateCommand CompleteUnsuccessful { get; }

        private string number;
        private bool hasExceededSpeedRestriction;
        private bool isComplete;

        public string Number
        {
            get => this.number;
            set => this.SetProperty(ref this.number, value);
        }
        public bool HasExceededSpeedRestriction
        {
            get => this.hasExceededSpeedRestriction;
            set => this.SetProperty(ref this.hasExceededSpeedRestriction, value);
        }
        public bool IsComplete
        {
            get => this.isComplete;
            set => this.SetProperty(ref this.isComplete, value);
        }

        private void StartAction()
        {
            this.participation.Start();
            this.Update();
        }
        private void ArriveAction()
        {
            this.participation.Arrive(DateTime.Now);
            this.Update();
        }
        private void InspectAction()
        {
            this.participation.Inspect(DateTime.Now);
            this.Update();
        }
        private void ReInspectAction()
        {
            this.participation.ReInspect(DateTime.Now);
            this.Update();
        }
        private void CompleteSuccessfulAction()
        {
            this.participation.CompleteSuccessful();
            this.IsComplete = this.participation.IsComplete;
            this.Update();
        }
        private void CompleteUnsuccessfulAction()
        {
            this.participation.CompleteUnsuccessful("code");
            this.IsComplete = this.participation.IsComplete;
            this.Update();
        }

        protected override async Task Load(int id)
        {
            if (this.Id != default)
            {
                return;
            }

            var query = new GetParticipation
            {
                Id = id,
            };
            this.participation = await this.Application.Execute(query);
            this.MapFrom(this.participation);
        }

        private async Task Update()
        {
            var update = new UpdateParticipation(this.participation);
            await this.Application.Execute(update);
        }
    }
}
