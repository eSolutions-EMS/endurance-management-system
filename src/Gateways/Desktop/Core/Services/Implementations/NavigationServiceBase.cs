using Prism.Regions;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Core.Services.Implementations
{
    public abstract class NavigationServiceBase
    {
        private static readonly Type ViewType = typeof(IView);

        private readonly IRegionManager regionManager;

        protected NavigationServiceBase(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        protected void ChangeTo<T>(string regionName) where T : IView
        {
            this.ChangeTo(regionName, typeof(T), null);
        }

        protected void ChangeTo(string regionName, Type view, NavigationParameters parameters)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (!ViewType.IsAssignableFrom(view))
            {
                throw new InvalidOperationException($"Type '{view?.Name}' does not implement '{ViewType}'");
            }

            var viewName = view.Name;
            this.regionManager.RequestNavigate(regionName, viewName, parameters);
        }

        protected void ClearRegion(string name)
        {
            var region = this.regionManager.Regions[name];
            region.RemoveAll();
        }
    }
}
