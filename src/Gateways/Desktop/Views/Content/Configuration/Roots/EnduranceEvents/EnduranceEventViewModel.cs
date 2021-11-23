using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Personnel;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.EnduranceEvents
{
    public class EnduranceEventViewModel : RelatedConfigurationBase<EnduranceEventView, EnduranceEvent>
    {
        private readonly ConfigurationManager manager;
        private readonly IEnduranceEventQuery enduranceEventQuery;
        private readonly IQueries<Country> countryQueries;

        public EnduranceEventViewModel(
            ConfigurationManager manager,
            IEnduranceEventQuery enduranceEventQuery,
            IQueries<Country> countryQueries) : base (enduranceEventQuery)
        {
            this.BackOnSubmit = false;
            this.manager = manager;
            this.enduranceEventQuery = enduranceEventQuery;
            this.countryQueries = countryQueries;
            this.CreateCompetition = new DelegateCommand(this.NewForm<CompetitionView>);
            this.CreatePersonnel = new DelegateCommand(this.NewForm<PersonnelView>);
        }

        public DelegateCommand CreatePersonnel { get; }
        public DelegateCommand CreateCompetition { get; }
        public ObservableCollection<SimpleListItemViewModel> Countries { get; } = new();
        public ObservableCollection<PersonnelViewModel> Personnel { get; } = new();
        public ObservableCollection<CompetitionViewModel> Competitions { get; } = new();

        private string name;
        private string populatedPlace;
        private int countryId;

        public override bool IsNavigationTarget(NavigationContext context)
            => true;
        public override void OnNavigatedTo(NavigationContext context)
        {
            this.Load(default); // Only one Endurance event per state.
            this.LoadCountries();
            this.Journal = context.NavigationService.Journal;
        }

        protected override void Load(int id)
        {
            this.Personnel.Clear();
            this.Competitions.Clear();
            var enduranceEvent = this.enduranceEventQuery.Get();
            this.MapFrom(enduranceEvent);
        }
        protected override IDomainObject Persist()
        {
            var result = this.manager.Update(this.Name, this.CountryId, this.PopulatedPlace);
            return result;
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
        public int CountryId
        {
            get => this.countryId;
            set => this.SetProperty(ref this.countryId, value);
        }

        private void LoadCountries()
        {
            if (this.Countries.Any())
            {
                return;
            }
            var countries = this.countryQueries.GetAll();
            var viewModels = countries.Select(x => new SimpleListItemViewModel(x.Id, x.Name));
            this.Countries.AddRange(viewModels);
        }
    }
}
