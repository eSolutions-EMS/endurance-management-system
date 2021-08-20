using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Participants;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Phases;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions
{
    public interface IPhasesShard<TView> : IPrincipalForm
        where TView : IView
    {
        ObservableCollection<ListItemViewModel> PhaseItems { get; }

        List<PhaseViewModel> Phases { get; }

        DelegateCommand NavigateToCreatePhase { get; }
    }

    public interface IParticipantsShard<TView> : IPrincipalForm
        where TView : IView
    {
        ObservableCollection<ListItemViewModel> ParticipantItems { get; }

        List<ParticipantViewModel> Participants { get; }

        DelegateCommand NavigateToCreateParticipant { get; }
    }
}
