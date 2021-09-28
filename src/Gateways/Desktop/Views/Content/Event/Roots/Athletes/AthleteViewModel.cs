using EnduranceJudge.Application.Events.Commands.Athletes;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Application.Events.Queries.GetAthlete;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes
{
    public class AthleteViewModel : RootFormBase<GetAthlete, UpdateAthlete, AthleteRootModel, AthleteView>,
        IAthleteState,
        IListable
    {
        private AthleteViewModel(IApplicationService application) : base(application)
        {
            this.CategoryId = (int)Category.Adults;
            this.CountryIsoCode = "BUL";
        }

        public ObservableCollection<SimpleListItemViewModel> CategoryItems { get; }
            = new(SimpleListItemViewModel.FromEnum<Category>());
        public ObservableCollection<CountryListModel> CountryItems { get; }
            = new(Enumerable.Empty<CountryListModel>());

        private string feiId;
        private string firstName;
        private string lastName;
        private string countryIsoCode;
        private int categoryId;

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.LoadCountries();
        }

        public string FeiId
        {
            get => this.feiId;
            set => this.SetProperty(ref this.feiId, value);
        }
        public string FirstName
        {
            get => this.firstName;
            set => this.SetProperty(ref this.firstName, value);
        }
        public string LastName
        {
            get => this.lastName;
            set => this.SetProperty(ref this.lastName, value);
        }
        public string CountryIsoCode
        {
            get => this.countryIsoCode;
            set => this.SetProperty(ref this.countryIsoCode, value);
        }
        public int CategoryId
        {
            get => this.categoryId;
            set => this.SetProperty(ref this.categoryId, value);
        }

        private async Task LoadCountries()
        {
            var countries = await this.Application.Execute(new GetCountriesList());
            this.CountryItems.AddRange(countries);
        }

        public Category Category => (Category)this.CategoryId;
        public string Name => $"{this.FirstName} {this.LastName}";
    }
}
