using EnduranceJudge.Application.Core.Models;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Events.Athletes;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes
{
    public class AthleteViewModel : FormBase<AthleteView>, IAthleteState, IListable
    {
        private readonly IEventAggregator eventAggregator;
        private AthleteViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.CategoryId = (int)Category.Adults;
            this.CountryIsoCode = "BUL";
        }

        public ObservableCollection<SimpleListItemViewModel> CategoryItems { get; }
            = new(SimpleListItemViewModel.FromEnum<Category>());
        public ObservableCollection<ListItemModel> CountryItems { get; }
            = new(Enumerable.Empty<ListItemModel>());

        private string feiId;
        private string firstName;
        private string lastName;
        private string countryIsoCode;
        private int categoryId;
        private string club;

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.LoadCountries();
        }

        protected override void Load(int id)
        {
            throw new NotImplementedException();
        }
        protected override void DomainAction()
        {
            // TODO: submit
            this.eventAggregator
                .GetEvent<AthleteUpdatedEvent>()
                .Publish(this);
        }
        private void LoadCountries()
        {
            throw new NotImplementedException();
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
        public string Club
        {
            get => this.club;
            set => this.SetProperty(ref this.club, value);
        }
        public Category Category => (Category)this.CategoryId;
        public string Name => $"{this.FirstName} {this.LastName}";
    }
}
