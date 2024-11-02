using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace EMS.Judge.Common;

public abstract class ViewModelBase : BindableBase, INavigationAware
{
    protected IRegionNavigationJournal Journal { get; set; }

    public DelegateCommand NavigateForward => new(this.NavigateForwardAction);
    public DelegateCommand NavigateBack => new(this.NavigateBackAction);

    public virtual void OnNavigatedTo(NavigationContext context)
    {
        this.Journal = context.NavigationService.Journal;
    }

    public virtual bool IsNavigationTarget(NavigationContext context)
    {
        return false;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

    protected virtual void NavigateForwardAction()
    {
        this.Journal.GoForward();
    }

    protected virtual void NavigateBackAction()
    {
        this.Journal.GoBack();
    }
}
