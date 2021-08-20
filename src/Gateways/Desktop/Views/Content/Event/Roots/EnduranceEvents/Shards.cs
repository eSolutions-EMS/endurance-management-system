using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Personnel;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public interface ICompetitionsShard<TView> : IPrincipalForm
        where TView : IView
    {
        ObservableCollection<ListItemViewModel> CompetitionItems { get; }

        List<CompetitionViewModel> Competitions { get; }

        DelegateCommand NavigateToCompetition { get; }
    }

    public interface IPersonnelShard<TView> : IPrincipalForm
        where TView : IView
    {
        ObservableCollection<ListItemViewModel> PersonnelItems { get; }

        List<PersonnelViewModel> Personnel { get; }

        DelegateCommand NavigateToPersonnel { get; }
    }
}
