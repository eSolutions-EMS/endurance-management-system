using EMS.Judge.Core;
using EMS.Judge.Services;
using Prism.Commands;

namespace EMS.Judge.Views.Navigation;

public class NavigationViewModel : ViewModelBase
{
    public NavigationViewModel(INavigationService navigation)
    {
        this.NavigateToImport = new DelegateCommand(navigation.NavigateToImport);
        this.NavigateToEvent = new DelegateCommand(navigation.NavigateToEvent);
        this.NavigateToManager = new DelegateCommand(navigation.NavigateToManager);
        this.NavigateToRanking = new DelegateCommand(navigation.NavigateToRanking);
        this.NavigateToHardware = new DelegateCommand(navigation.NavigateToHardware);
    }

    public DelegateCommand NavigateToImport { get; }
    public DelegateCommand NavigateToEvent { get; }
    public DelegateCommand NavigateToManager { get; }
    public DelegateCommand NavigateToRanking { get; }
    public DelegateCommand NavigateToHardware { get; }
}
