using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.PhasesForCategory;
using EnduranceJudge.Localization;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Phases
{
    public class PhaseViewModel : DependantFormBase,
        IPhaseForCategoryShard<PhaseForCategoryView>
    {
        private PhaseViewModel() : base(null, null)
        {
        }

        public PhaseViewModel(IApplicationService application, INavigationService navigation)
            : base(application, navigation)
        {
        }

        public bool IsFinal => this.isFinalValue != 0;

        private int isFinalValue;
        public int IsFinalValue
        {
            get => this.isFinalValue;
            set => this.SetProperty(ref this.isFinalValue, value);
        }

        private int lengthInKm;
        public int LengthInKm
        {
            get => this.lengthInKm;
            set => this.SetProperty(ref this.lengthInKm, value);
        }

        protected override ListItemViewModel ToListItem(DelegateCommand command)
        {
            var display = string.Format(DesktopStrings.PhaseListItemDisplayTemplate, this.lengthInKm, this.IsFinal);
            return new ListItemViewModel(this.Id, display, command);
        }

        public ObservableCollection<ListItemViewModel> PhaseForCategoryItems { get; } = new();
        public List<PhaseForCategoryViewModel> PhasesForCategories { get; private set; } = new();
        public DelegateCommand NavigateToCreatePhaseForCategory { get; private set; }
    }
}
