using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Application.Events.Queries.GetAthletesList;
using EnduranceJudge.Application.Events.Queries.GetHorseList;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Participants
{
    public class ParticipantViewModel : ChildFormBase<ParticipantView>,
        IMap<ParticipantDependantModel>
    {
        private readonly IApplicationService application;
        private ParticipantViewModel()
        {
        }

        public ParticipantViewModel(IApplicationService application)
        {
            this.application = application;
            this.ToggleIsAverageSpeedInKmPhVisibility = new DelegateCommand(
                this.ToggleIsAverageSpeedInKmPhVisibilityAction);
        }

        public ObservableCollection<SimpleListItemViewModel> HorseItems { get; } = new();
        public ObservableCollection<SimpleListItemViewModel> AthleteItems { get; } = new();

        private string rfId;
        public int number;
        public int? maxAverageSpeedInKmPh;
        private Visibility maxAverageSpeedInKmPhVisibility = Visibility.Hidden;
        private int horseId;
        private int athleteId;
        private int categoryId;
        private string name;
        private string horseName;
        private string athleteName;

        public string RfId
        {
            get => this.rfId;
            set => this.SetProperty(ref this.rfId, value);
        }
        public int Number
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
        public int CategoryId
        {
            get => this.categoryId;
            set => this.SetProperty(ref this.categoryId, value);
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

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);
            this.LoadCompetitors();
            if (this.MaxAverageSpeedInKmPh.HasValue)
            {
                this.ShowMaxAverageSpeedInKmPh();
            }
        }

        public DelegateCommand ToggleIsAverageSpeedInKmPhVisibility { get; private set; }
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

        private async Task LoadCompetitors()
        {
            var horses = await this.application.Execute(new GetHorseList());
            var athletes = await this.application.Execute(new GetAthletesList());

            var horseItems = horses.Select(x => new SimpleListItemViewModel(x));
            var athleteItems = athletes.Select(x => new SimpleListItemViewModel(x));

            this.HorseItems.AddRange(horseItems);
            this.AthleteItems.AddRange(athleteItems);
        }

        private string FormatName()
        {
            return string.Format(DesktopConstants.COMPOSITE_NAME_FORMAT, this.AthleteName, this.horseName);
        }
    }
}
