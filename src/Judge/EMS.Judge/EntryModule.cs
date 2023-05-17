using EMS.Judge.Common;
using EMS.Judge.Services;
using Prism.Ioc;

namespace EMS.Judge;

public class EntryModule : ModuleBase
{
    public override void OnInitialized(IContainerProvider containerProvider)
    {
        base.OnInitialized(containerProvider);

        var navigation = containerProvider.Resolve<INavigationService>();
        navigation.NavigateToImport();
    }
}
