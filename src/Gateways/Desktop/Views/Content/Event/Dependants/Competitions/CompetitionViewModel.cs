using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Participants;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Phases;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions
{
    public class CompetitionViewModel : DependantFormBase, IMap<CompetitionDependantModel>,
        IPhasesShard<PhaseView>,
        IParticipantsShard<ParticipantView>
    {
        public CompetitionViewModel() : base(null, null)
        {
            this.LoadTypes();
        }

        public CompetitionViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
            this.LoadTypes();
        }

        public ObservableCollection<SimpleListItemViewModel> TypeItems { get; private set; }
        public int PhaseCount => this.Phases.Count;

        private int type;
        public int Type
        {
            get => this.type;
            set => this.SetProperty(ref this.type, value);
        }

        private string name;
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        private void LoadTypes()
        {
            var typeViewModels = SimpleListItemViewModel.FromEnum<CompetitionType>();
            this.TypeItems = new ObservableCollection<SimpleListItemViewModel>(typeViewModels);
        }

        protected override ListItemViewModel ToListItem(DelegateCommand command)
        {
            var listItem = new ListItemViewModel(this.Id, this.Name, command);
            return listItem;
        }

        public ObservableCollection<ListItemViewModel> PhaseItems { get; } = new();
        public List<PhaseViewModel> Phases { get; private set; } = new();
        public DelegateCommand NavigateToCreatePhase { get; private set; }

        public ObservableCollection<ListItemViewModel> ParticipantItems { get; } = new();
        public List<ParticipantViewModel> Participants { get; private set; } = new();
        public DelegateCommand NavigateToCreateParticipant { get; private set; }
    }
}
