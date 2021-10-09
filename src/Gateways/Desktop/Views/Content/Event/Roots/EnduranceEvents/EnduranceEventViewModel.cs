using EnduranceJudge.Application.Contracts.Queries;
using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public class EnduranceEventViewModel : RootFormBase<EnduranceEventView>
    {
        private readonly IEnduranceEventQuery enduranceEventQuery;
        private readonly IQueries<Country> countryQueries;
        public EnduranceEventViewModel(IEnduranceEventQuery enduranceEventQuery, IQueries<Country> countryQueries)
        {
            this.enduranceEventQuery = enduranceEventQuery;
            this.countryQueries = countryQueries;
            this.NavigateToCompetition = new DelegateCommand(this.NavigateToNewChild<CompetitionView>);
            this.NavigateToPersonnel = new DelegateCommand(this.NavigateToNewChild<PersonnelView>);
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
        protected override void SubmitAction()
        {
            throw new NotImplementedException();
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
            var countries = this.countryQueries.GetAll();
            var viewModels = countries.Select(x => new ListItemModel { Id = x.Id, Name = x.Name });
            this.CountryItems.AddRange(viewModels);
        }

        public override void HandleChildren(NavigationContext context)
        {
            this.AddOrUpdateChild(context, this.Personnel);
            this.AddOrUpdateChild(context, this.Competitions);

            this.UpdateGrandChild(context, this.Competitions);
        }
    }
}
