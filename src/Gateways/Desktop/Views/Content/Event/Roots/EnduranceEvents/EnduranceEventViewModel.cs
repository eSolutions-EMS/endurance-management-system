using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public class EnduranceEventViewModel : FormBase<EnduranceEventView>
    {
        private readonly IDomainHandler domainHandler;
        private readonly IPersistence persistence;
        private readonly IEnduranceEventQuery enduranceEventQuery;
        private readonly IQueries<Country> countryQueries;
        public EnduranceEventViewModel(
            IDomainHandler domainHandler,
            IPersistence persistence,
            IEnduranceEventQuery enduranceEventQuery,
            IQueries<Country> countryQueries)
        {
            this.domainHandler = domainHandler;
            this.persistence = persistence;
            this.enduranceEventQuery = enduranceEventQuery;
            this.countryQueries = countryQueries;
            this.NavigateToCompetition = new DelegateCommand(this.Navigation.ChangeTo<CompetitionView>);
            this.NavigateToPersonnel = new DelegateCommand(this.Navigation.ChangeTo<PersonnelView>);
        }

        public DelegateCommand NavigateToPersonnel { get; }
        public DelegateCommand NavigateToCompetition { get; }
        public ObservableCollection<ListItemModel> CountryItems { get; } = new();
        public ObservableCollection<PersonnelViewModel> Personnel { get; } = new();
        public ObservableCollection<CompetitionViewModel> Competitions { get; } = new();

        private string name;
        private string populatedPlace;
        private int countryId;

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.LoadCountries();
        }

        protected override void Load(int id)
        {
            var enduranceEvent = this.enduranceEventQuery.Get();
            this.MapFrom(enduranceEvent);
        }
        protected override void SubmitAction() => this.domainHandler.Handle(() =>
        {
            var manager = new ConfigurationManager();
            manager.Update(this.Name, this.CountryId, this.PopulatedPlace);
            this.persistence.Update(manager);
        });

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
            var countries = this.countryQueries.GetAll();
            var viewModels = countries.Select(x => new ListItemModel { Id = x.Id, Name = x.Name });
            this.CountryItems.AddRange(viewModels);
        }
    }
}
