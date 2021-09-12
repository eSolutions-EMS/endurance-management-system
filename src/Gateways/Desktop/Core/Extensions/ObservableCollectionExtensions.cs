using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Core.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void HandleChildren<T>(this ObservableCollection<T> parents, NavigationContext context)
            where T : IParentForm
        {
            foreach (var parent in parents)
            {
                parent.HandleChildren(context);
            }
        }
    }
}
