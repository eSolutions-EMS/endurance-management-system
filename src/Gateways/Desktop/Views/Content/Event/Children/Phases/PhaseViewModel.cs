using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.PhasesForCategory;
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

        private int isFinalValue;
        private int lengthInKm;

        public bool IsFinal => this.isFinalValue != 0;

        public int IsFinalValue
        {
            get => this.isFinalValue;
            set => this.SetProperty(ref this.isFinalValue, value);
        }
        public int LengthInKm
        {
            get => this.lengthInKm;
            set => this.SetProperty(ref this.lengthInKm, value);
        }

        protected override void HandleChildren(NavigationContext context)
        {
            var phaseForCategory = context.GetChild<PhaseForCategoryViewModel>();
            if (phaseForCategory != null)
            {
                this.PhasesForCategories.AddOrUpdateObject(phaseForCategory);
            }
        }
    }
}
