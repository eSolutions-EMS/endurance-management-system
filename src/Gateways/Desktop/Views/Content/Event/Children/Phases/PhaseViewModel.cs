using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.PhasesForCategory;
using EnduranceJudge.Localization;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Phases
{
    public class PhaseViewModel : ChildFormBase<PhaseView>
    {
        private PhaseViewModel()
        {
            this.NavigateToCreatePhaseForCategory = new DelegateCommand(
                this.NavigateToNewChild<PhaseForCategoryView>);
        }

        public DelegateCommand NavigateToCreatePhaseForCategory { get; }
        public ObservableCollection<PhaseForCategoryViewModel> PhasesForCategories { get; } = new();

        private string isFinalText;
        private int isFinalValue;
        private int lengthInKm;

        public string IsFinalText
        {
            get => this.isFinalText;
            set => this.SetProperty (ref this.isFinalText, value);
        }

        public int IsFinalValue
        {
            get => this.isFinalValue;
            set
            {
                this.SetProperty(ref this.isFinalValue, value);
                this.IsFinalText = value == 1
                    ? DesktopStrings.IsFinalText
                    : string.Empty;
            }
        }
        public int LengthInKm
        {
            get => this.lengthInKm;
            set => this.SetProperty(ref this.lengthInKm, value);
        }

        public override void HandleChildren(NavigationContext context)
        {
            this.AddOrUpdateChild(context, this.PhasesForCategories);
        }
    }
}
