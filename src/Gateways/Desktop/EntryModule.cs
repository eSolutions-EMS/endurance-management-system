using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Ioc;

namespace EnduranceJudge.Gateways.Desktop
{
    public class EntryModule : ModuleBase
    {
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);

            var navigation = containerProvider.Resolve<INavigationService>();
            navigation.NavigateToImport();
        }
    }
}
