using EnduranceJudge.Application.Events.Commands.EnduranceEvents;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Application.Events.Queries.GetEvent;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public class EnduranceEventViewModel
        : RootFormBase<GetEnduranceEvent, SaveEnduranceEventState, EnduranceEventRootModel, EnduranceEventView>
    {
        private EnduranceEventViewModel(IApplicationService application)  : base(application)
        {
            this.NavigateToCompetition = new DelegateCommand(this.NavigateToNewChild<CompetitionView>);
            this.NavigateToPersonnel = new DelegateCommand(this.NavigateToNewChild<PersonnelView>);
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

        public override void HandleChildren(NavigationContext context)
        {
            this.AddOrUpdateChild(context, this.Personnel);
            this.AddOrUpdateChild(context, this.Competitions);

            this.UpdateGrandChild(context, this.Competitions);
        }
    }
}
