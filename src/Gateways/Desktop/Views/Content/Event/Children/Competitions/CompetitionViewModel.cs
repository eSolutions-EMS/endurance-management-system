using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Participants;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Phases;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Competitions
{
    public class CompetitionViewModel : ChildFormBase<CompetitionView>
    {
        public DelegateCommand NavigateToCreatePhase { get; private set; }
        public DelegateCommand NavigateToCreateParticipant { get; private set; }
        public ObservableCollection<SimpleListItemViewModel> TypeItems { get; private set; }
        public ObservableCollection<PhaseViewModel> Phases { get; } = new();
        public ObservableCollection<ParticipantViewModel> Participants { get; } = new();

        private int type;
        private string name;

        public int PhaseCount => this.Phases.Count;

        public int Type
        {
            get => this.type;
            set => this.SetProperty(ref this.type, value);
        }
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        protected override void Initialize()
        {
            this.LoadTypes();
            this.NavigateToCreatePhase = new DelegateCommand(this.NavigateToNewChild<PhaseView>);
            this.NavigateToCreateParticipant = new DelegateCommand(this.NavigateToNewChild<ParticipantView>);
        }

        protected override void HandleChildren(NavigationContext context)
        {
            var phase = context.GetChild<PhaseViewModel>();
            if (phase != null)
            {
                this.Phases.AddOrUpdateObject(phase);
            }
            var participant = context.GetChild<ParticipantViewModel>();
            if (participant != null)
            {
                this.Participants.AddOrUpdateObject(participant);
            }
        }

        private void LoadTypes()
        {
            var typeViewModels = SimpleListItemViewModel.FromEnum<CompetitionType>();
            this.TypeItems = new ObservableCollection<SimpleListItemViewModel>(typeViewModels);
        }
    }
}
