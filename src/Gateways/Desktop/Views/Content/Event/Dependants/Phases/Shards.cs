using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.PhasesForCategory;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Phases
{
    public interface IPhaseForCategoryShard<TView> : IPrincipalForm
    {
        ObservableCollection<ListItemViewModel> PhaseForCategoryItems { get; }

        List<PhaseForCategoryViewModel> PhasesForCategories { get; }

        DelegateCommand NavigateToCreatePhaseForCategory { get; }
    }
}
