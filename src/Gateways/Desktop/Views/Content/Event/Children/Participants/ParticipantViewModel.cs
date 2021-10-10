using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Participants
{
    public class ParticipantViewModel : FormBase<ParticipantView, Participant>, IMap<Participant>
    {

        private ParticipantViewModel() : base(null) {}
        public ParticipantViewModel(IQueries<Participant> participants) : base(participants)
        {
            this.ToggleIsAverageSpeedInKmPhVisibility = new DelegateCommand(
                this.ToggleIsAverageSpeedInKmPhVisibilityAction);
        }

        public DelegateCommand ToggleIsAverageSpeedInKmPhVisibility { get; }

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

        protected override void Load(int id)
        {
            throw new NotImplementedException();
        }
        protected override void ActOnSubmit()
        {
            throw new NotImplementedException();
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
            return string.Format(DesktopConstants.COMPOSITE_NAME_FORMAT, this.AthleteName, this.horseName);
        }

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
    }
}
