using EMS.Judge.Core.Components.Templates.SimpleListItem;
using EMS.Judge.Services;
using EMS.Judge.Views.Content.Configuration.Core;
using EMS.Judge.Application.Core;
using EMS.Judge.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Models;
using EMS.Core.Domain.AggregateRoots.Configuration;
using EMS.Core.Domain.Core.Models;
using EMS.Core.Domain.State.Athletes;
using EMS.Core.Domain.Enums;
using EMS.Core.Domain.State.Countries;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using static EMS.Core.Localization.LocalizationConstants;

namespace EMS.Judge.Views.Content.Configuration.Roots.Athletes;

public class AthleteViewModel : ConfigurationBase<AthleteView, Athlete>, IAthleteState, IListable
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private readonly IQueries<Country> countries;
    private readonly IQueries<Athlete> athletes;

    private AthleteViewModel(
        IExecutor<ConfigurationRoot> executor,
        IQueries<Country> countries,
        IQueries<Athlete> athletes) : base(athletes)
    {
        this.executor = executor;
        this.countries = countries;
        this.athletes = athletes;
        this.CategoryId = (int)Category.Adults;
    }

    public ObservableCollection<SimpleListItemViewModel> CategoryItems { get; }
        = new(SimpleListItemViewModel.FromEnum<Category>());
    public ObservableCollection<ListItemModel> CountryItems { get; } = new();

    private string feiId;
    private string firstName;
    private string lastName;
    private int countryId;
    private int categoryId;
    private string club;

    public override void OnNavigatedTo(NavigationContext context)
    {
        base.OnNavigatedTo(context);
        this.LoadCountries();
    }

    protected override void Load(int id)
    {
        var athlete = this.athletes.GetOne(id);
        this.MapFrom(athlete);
    }
    protected override IDomain Persist()
    {
        var result = this.executor.Execute(
            config => config.Athletes.Save(this, this.CountryId),
            true);
        return result;
    }
    private void LoadCountries()
    {
        var countries = this.countries.GetAll();

        var listItems = countries.MapEnumerable<ListItemModel>();
        this.CountryItems.AddRange(listItems);
        if (this.countryId == default)
        {
            this.CountryId = countries
                .First(x => x.IsoCode == DEFAULT_COUNTRY_CODE)
                .Id;
        }
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
    public int CountryId
    {
        get => this.countryId;
        set => this.SetProperty(ref this.countryId, value);
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
