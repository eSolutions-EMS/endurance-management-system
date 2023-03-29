using EnduranceJudge.Application.Core;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Participants;

public class ParticipantViewModel : ConfigurationBase<ParticipantView, Participant>,
    IParticipantState,
    IMapFrom<Participant>
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private readonly IQueries<Athlete> athletes;
    private readonly IQueries<Horse> horses;
    private ParticipantViewModel() : base(null) {}
    public ParticipantViewModel(
        IExecutor<ConfigurationRoot> executor,
        IQueries<Athlete> athletes,
        IQueries<Horse> horses,
        IQueries<Participant> participants) : base(participants)
    {
        this.executor = executor;
        this.athletes = athletes;
        this.horses = horses;
        this.ToggleIsAverageSpeedInKmPhVisibility = new DelegateCommand(
            this.ToggleIsAverageSpeedInKmPhVisibilityAction);
    }

    public DelegateCommand ToggleIsAverageSpeedInKmPhVisibility { get; }

    public ObservableCollection<SimpleListItemViewModel> HorseItems { get; } = new();
    public ObservableCollection<SimpleListItemViewModel> AthleteItems { get; } = new();

    private string rfidHead;
    private string rfidNeck;
    public string number;
    public int? maxAverageSpeedInKmPh;
    private Visibility maxAverageSpeedInKmPhVisibility = Visibility.Hidden;
    private int horseId;
    private int athleteId;
    private string name;
    private string horseName;
    private string athleteName;

    public override void OnNavigatedTo(NavigationContext context)
    {
        this.LoadAthletes();
        this.LoadHorses();
        base.OnNavigatedTo(context);
    }

    protected override IDomain Persist()
    {
        var result = this.executor.Execute(
            config => config.Participants.Save(this, this.AthleteId, this.HorseId),
            true);
        return result;
    }

    private void LoadAthletes()
    {
        var athletes = this.athletes.GetAll();
        var viewModels = athletes.MapEnumerable<SimpleListItemViewModel>();
        this.AthleteItems.Clear();
        this.AthleteItems.AddRange(viewModels);
    }
    private void LoadHorses()
    {
        var horses = this.horses.GetAll();
        var viewModels = horses.MapEnumerable<SimpleListItemViewModel>();
        this.HorseItems.Clear();
        this.HorseItems.AddRange(viewModels);
    }

    public void ToggleIsAverageSpeedInKmPhVisibilityAction()
    {
        if (this.MaxAverageSpeedInKmPhVisibility == Visibility.Hidden)
        {
            this.ShowMaxAverageSpeedInKmPh();
        }
        else
        {
            this.HideMaxAverageSpeedInKmPh();
        }
    }
    private void ShowMaxAverageSpeedInKmPh()
    {
        this.MaxAverageSpeedInKmPhVisibility = Visibility.Visible;
        this.MaxAverageSpeedInKmPh = Participant.DEFAULT_MAX_AVERAGE_SPEED;
    }
    private void HideMaxAverageSpeedInKmPh()
    {
        this.MaxAverageSpeedInKmPhVisibility = Visibility.Hidden;
        this.MaxAverageSpeedInKmPh = default;
    }
    public Visibility MaxAverageSpeedInKmPhVisibility
    {
        get => this.maxAverageSpeedInKmPhVisibility;
        set => this.SetProperty(ref this.maxAverageSpeedInKmPhVisibility, value);
    }

    private string FormatName()
    {
        return Participant.FormatName(this.Number, this.AthleteName, this.horseName);
    }

    public string RfIdHead
    {
        get => this.rfidHead;
        set => this.SetProperty(ref this.rfidHead, value);
    }
    public string RfIdNeck
    {
        get => this.rfidNeck;
        set => this.SetProperty(ref this.rfidNeck, value);
    }
    public string Number
    {
        get => this.number;
        set => this.SetProperty(ref this.number, value);
    }
    public int? MaxAverageSpeedInKmPh
    {
        get => this.maxAverageSpeedInKmPh;
        set => this.SetProperty(ref this.maxAverageSpeedInKmPh, value);
    }
    public int HorseId
    {
        get => this.horseId;
        set => this.SetProperty(ref this.horseId, value);
    }
    public int AthleteId
    {
        get => this.athleteId;
        set => this.SetProperty(ref this.athleteId, value);
    }
    public string HorseName
    {
        get => this.horseName;
        set
        {
            this.SetProperty(ref this.horseName, value);
            this.Name = this.FormatName();
        }
    }
    public string AthleteName
    {
        get => this.athleteName;
        set
        {
            this.SetProperty(ref this.athleteName, value);
            this.Name = this.FormatName();
        }
    }
    public string Name
    {
        get => this.name;
        private set => this.SetProperty(ref this.name, value);
    }
}
