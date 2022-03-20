using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;

namespace EnduranceJudge.Gateways.Desktop.Views.Navigation;

public class NavigationViewModel : ViewModelBase
{
    public NavigationViewModel(IExecutor<INavigationService> navigation)
    {
        this.NavigateToImport = new DelegateCommand(() => navigation.Execute(nav =>  nav.NavigateToImport()));
        this.NavigateToEvent = new DelegateCommand(() => navigation.Execute(nav =>  nav.NavigateToEvent()));
        this.NavigateToManager = new DelegateCommand(() => navigation.Execute(nav =>  nav.NavigateToManager()));
        this.NavigateToRanking = new DelegateCommand(() => navigation.Execute(nav =>  nav.NavigateToRanking()));
    }

    public DelegateCommand NavigateToImport { get; }
    public DelegateCommand NavigateToEvent { get; }
    public DelegateCommand NavigateToManager { get; }
    public DelegateCommand NavigateToRanking { get; }
}
