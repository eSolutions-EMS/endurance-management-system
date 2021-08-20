using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents.Listing;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.NavigationStrip;
using EnduranceJudge.Gateways.Desktop.Views.Content.Import;
using EnduranceJudge.Gateways.Desktop.Core.Services.Implementations;
using Prism.Regions;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services.Implementations
{
    public class NavigationService : NavigationServiceBase, INavigationService
    {
        public NavigationService(IRegionManager regionManager) : base(regionManager)
        {
        }

        public void NavigateToImport()
        {
            this.ChangeTo<ImportView>();
            this.ClearRegion(Regions.SubNavigation);
        }

        public void NavigateToEvent()
        {
            this.ChangeTo<EnduranceEventListView>();
            this.ChangeTo<EventNavigationStripView>(Regions.SubNavigation);
        }

        public void ChangeTo<T>() where T : IView
        {
            this.ChangeTo(Regions.Content, typeof(T));
        }

        public void ChangeTo<T>(int id) where T : IView
        {
            this.ChangeTo(Regions.Content, typeof(T), id);
        }

        public void ChangeTo<TView>(Action<object> action) where TView : IView
        {
            this.ChangeTo(typeof(TView), action);
        }

        public void ChangeTo(Type viewType, Action<object> action)
        {
            this.ChangeTo(Regions.Content, viewType, action);
        }

        public void ChangeTo<TView>(object data, Action<object> action) where TView : IView
        {
            this.ChangeTo(typeof(TView), data, action);
        }

        public void ChangeTo(Type viewType, object data, Action<object> action)
        {
            this.ChangeTo(Regions.Content, viewType, data, action);
        }
    }
}
