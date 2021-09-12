using EnduranceJudge.Core.Models;
using Prism.Regions;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public interface IParentForm
    {
        public void HandleChildren(NavigationContext context);
    }
}
