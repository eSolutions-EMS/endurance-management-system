using EnduranceJudge.Application.Events.Commands.EnduranceEvents;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Application.Events.Queries.GetEvent;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Personnel;
using MediatR;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public class EnduranceEventViewModel : RootFormBase<SaveEnduranceEvent, EnduranceEventRootModel>,
        ICompetitionsShard<CompetitionView>,
        IPersonnelShard<PersonnelView>
    {
        protected EnduranceEventViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }

        public ObservableCollection<CountryListModel> Countries { get; }
            = new (Enumerable.Empty<CountryListModel>());

        private string name;
        private string populatedPlace;
        private string selectedCountryIsoCode;

        public Visibility CountryVisibility { get; private set; } = Visibility.Hidden;

        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        public string PopulatedPlace
        {
            get => this.populatedPlace;
            set => this.SetProperty(ref this.populatedPlace, value);
        }
        public string SelectedCountryIsoCode
        {
            get => this.selectedCountryIsoCode;
            set => this.SetProperty(ref this.selectedCountryIsoCode, value);
        }

        protected override IRequest<EnduranceEventRootModel> LoadCommand(int id)
        {
            return new GetEnduranceEvent
            {
                Id = id,
            };
        }

        protected override ListItemViewModel ToListItem(DelegateCommand command)
        {
            var listItem = new ListItemViewModel(this.Id, this.Name, command);
            return listItem;
        }

        public DelegateCommand NavigateToPersonnel { get; private set; }
        public List<PersonnelViewModel> Personnel { get; private set; } = new();
        public ObservableCollection<ListItemViewModel> PersonnelItems { get; } = new();

        public DelegateCommand NavigateToCompetition { get; private set; }
        public List<CompetitionViewModel> Competitions { get; private set; } = new();
        public ObservableCollection<ListItemViewModel> CompetitionItems { get; } = new();
    }
}
