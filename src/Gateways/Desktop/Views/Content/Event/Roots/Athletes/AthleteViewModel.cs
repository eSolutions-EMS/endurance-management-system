using EnduranceJudge.Application.Events.Commands.UpdateAthlete;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Application.Events.Queries.GetAthlete;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ComboBoxItem;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using MediatR;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes
{
    public class AthleteViewModel : RootFormBase<UpdateAthlete, AthleteRootModel>,
        IAthleteState,
        IListable
    {
        public AthleteViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }

        public ObservableCollection<ComboBoxItemViewModel> CategoryItems { get; }
            = new(ComboBoxItemViewModel.FromEnum<Category>());
        public ObservableCollection<CountryListModel> CountryItems { get; }
            = new(Enumerable.Empty<CountryListModel>());

        protected override ListItemViewModel ToListItem(DelegateCommand command)
            => new(this, command);

        protected override IRequest<AthleteRootModel> LoadCommand(int id)
            => new GetAthlete { Id = id };

        private string feiId;
        private string firstName;
        private string lastName;
        private string countryIsoCode;
        private int categoryId;

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

        public Category Category => (Category)this.CategoryId;
        public string Name => $"{this.FirstName} {this.LastName}";
    }
}
