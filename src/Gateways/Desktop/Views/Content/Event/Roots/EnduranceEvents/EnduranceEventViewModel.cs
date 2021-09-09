using EnduranceJudge.Application.Events.Commands.EnduranceEvents;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Application.Events.Queries.GetEvent;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Personnel;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public class EnduranceEventViewModel
        : RootFormBase<GetEnduranceEvent, SaveEnduranceEvent, EnduranceEventRootModel, EnduranceEventView>
    {
        private EnduranceEventViewModel(IApplicationService application)  : base(application)
        {
            this.NavigateToCompetition = new DelegateCommand(this.NavigateToDependantCreate<CompetitionView>);
            this.NavigateToPersonnel = new DelegateCommand(this.NavigateToDependantCreate<PersonnelView>);
        }

        public DelegateCommand NavigateToPersonnel { get; }
        public DelegateCommand NavigateToCompetition { get; }
        public ObservableCollection<CountryListModel> Countries { get; }
            = new (Enumerable.Empty<CountryListModel>());
        public ObservableCollection<PersonnelViewModel> Personnel { get; } = new();
        public ObservableCollection<CompetitionViewModel> Competitions { get; } = new();

        private string name;
        private string populatedPlace;
        private string selectedCountryIsoCode;

        public Visibility CountryVisibility { get; private set; } = Visibility.Hidden;

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            if (!this.Countries.Any())
            {
                this.LoadCountries();
            }
        }

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

        private async Task LoadCountries()
        {
            var countries = await this.Application.Execute(new GetCountriesList());
            this.Countries.AddRange(countries);
        }

        protected override void HandleChildren(NavigationContext context)
        {
            var personnel = context.GetDependant<PersonnelViewModel>();
            if (personnel != null)
            {
                this.Personnel.AddOrUpdateObject(personnel);
            }
            var competition = context.GetDependant<CompetitionViewModel>();
            if (competition != null)
            {
                this.Competitions.AddOrUpdateObject(competition);
            }
        }
    }
}
