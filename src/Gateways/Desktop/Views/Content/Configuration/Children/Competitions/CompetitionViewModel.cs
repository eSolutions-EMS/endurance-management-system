using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.SimpleListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Phases;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions
{
    public class CompetitionViewModel : RelatedConfigurationBase<CompetitionView, Competition>, ICompetitionState
    {
        private readonly ConfigurationManager configuration;
        private readonly IExecutor<ConfigurationManager> executor;
        public CompetitionViewModel() : this(null, null) { }
        public CompetitionViewModel(
            ConfigurationManager configuration,
            IQueries<Competition> competitions) : base(competitions)
        {
            this.configuration = configuration;
            this.Toggle = new DelegateCommand(this.ToggleAction);
            this.CreatePhase = new DelegateCommand(this.NewForm<PhaseView>);
        }

        public DelegateCommand Toggle { get; }
        public DelegateCommand CreatePhase { get; }
        public ObservableCollection<SimpleListItemViewModel> TypeItems { get; }
            = new(SimpleListItemViewModel.FromEnum<CompetitionType>());
        public ObservableCollection<PhaseViewModel> Phases { get; } = new();

        private int typeValue;
        private string typeString;
        private string name;
        private DateTime startTime = DateTime.Today;
        public CompetitionType Type => (CompetitionType)this.TypeValue;
        private string toggleText = "Expand";
        private Visibility toggleVisibility = Visibility.Collapsed;

        protected override IDomainObject Persist()
        {
            var result = this.configuration.Competitions.Save(this);
            return result;
        }

        private void ToggleAction()
        {
            if (this.ToggleVisibility == Visibility.Collapsed)
            {
                this.ToggleVisibility = Visibility.Visible;
                this.ToggleText = "Collapse";
            }
            else
            {
                this.ToggleVisibility = Visibility.Collapsed;
                this.ToggleText = "Expand";
            }
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
        public Visibility ToggleVisibility
        {
            get => this.toggleVisibility;
            private set => this.SetProperty(ref this.toggleVisibility, value);
        }
    }
}
