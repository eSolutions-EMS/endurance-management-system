using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager.PhasePerformances;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participants
{
    public class ParticipantViewModel : ViewModelBase
    {
        public ParticipantViewModel(int number, Participation participation)
        {
            this.number = number;
            this.Participation = participation;
            this.UpdatePhases(participation);
        }

        private int number;
        private Visibility visibility = Visibility.Visible;
        public ObservableCollection<PhasePerformanceViewModel> PhasePerformances { get; } = new();
        public ObservableCollection<int> PhaseLengths { get; } = new();

        public Participation Participation { get; }
        public int Number
        {
            get => this.number;
            private set => this.SetProperty(ref this.number, value);
        }
        public Visibility Visibility
        {
            get => this.visibility;
            set => this.SetProperty(ref this.visibility, value);
        }

        private void UpdatePhases(Participation participation)
        {
            var participationInCompetition = participation.ParticipationsInCompetitions[0];
            var lengths = participationInCompetition.Phases.Select(x => x.LengthInKm);
            var viewModels = participationInCompetition
                .PhasePerformances
                .MapEnumerable<PhasePerformanceViewModel>();

            this.PhasePerformances.AddRange(viewModels);
            this.PhaseLengths.AddRange(lengths);
        }
    }
}
