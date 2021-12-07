using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Phases;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions
{
    public class CompetitionViewModel : RelatedConfigurationBase<CompetitionView, Competition>,
        ICompetitionState,
        ICollapsable
    {
        private readonly ConfigurationManager configuration;
        public CompetitionViewModel() : this(null, null) { }
        public CompetitionViewModel(
            ConfigurationManager configuration,
            IQueries<Competition> competitions) : base(competitions)
        {
            this.configuration = configuration;
            this.ToggleVisibility = new DelegateCommand(this.ToggleVisibilityAction);
            this.CreatePhase = new DelegateCommand(this.NewForm<PhaseView>);
        }

        public DelegateCommand ToggleVisibility { get; }
        public DelegateCommand CreatePhase { get; }
        public ObservableCollection<SimpleListItemViewModel> TypeItems { get; }
            = new(SimpleListItemViewModel.FromEnum<CompetitionType>());
        public ObservableCollection<PhaseViewModel> Phases { get; } = new();

        private int typeValue;
        private string name;
        private string typeString;
        private string toggleText = EXPAND;
        private DateTime startTime = DateTime.Today;
        private Visibility visibility = Visibility.Collapsed;
        public CompetitionType Type => (CompetitionType)this.TypeValue;

        protected override IDomainObject Persist()
        {
            var result = this.configuration.Competitions.Save(this);
            return result;
        }

        public string TypeString
        {
            get => this.typeString;
            set => this.SetProperty(ref this.typeString, value);
        }
        public int TypeValue
        {
            get => this.typeValue;
            set
            {
                this.SetProperty(ref this.typeValue, value);
                this.TypeString = ((CompetitionType)this.typeValue).ToString();
            }
        }
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        public DateTime StartTime
        {
            get => this.startTime;
            set => this.SetProperty(ref this.startTime, value);
        }
        public string ToggleText
        {
            get => this.toggleText;
            set => this.SetProperty(ref this.toggleText, value);
        }
        public Visibility Visibility
        {
            get => this.visibility;
            private set => this.SetProperty(ref this.visibility, value);
        }

        private void ToggleVisibilityAction()
        {
            if (this.Visibility == Visibility.Collapsed)
            {
                this.Visibility = Visibility.Visible;
                this.ToggleText = COLLAPSE;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                this.ToggleText = EXPAND;
            }
        }
    }
}
