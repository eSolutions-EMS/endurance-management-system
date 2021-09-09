using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Competitions;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public interface ICompetitionsShard<TView> : IParentForm
        where TView : IView
    {
        ObservableCollection<ListItemViewModel> CompetitionItems { get; }

        List<CompetitionViewModel> Competitions { get; }

        DelegateCommand NavigateToCompetition { get; }
    }

    public interface IPersonnelShard<TView> : IParentForm
        where TView : IView
    {
        ObservableCollection<ListItemViewModel> PersonnelItems { get; }

        List<PersonnelViewModel> Personnel { get; }

        DelegateCommand NavigateToPersonnel { get; }
    }
}
