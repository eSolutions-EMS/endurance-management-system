using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.ConfigurationMenu;
using EnduranceJudge.Gateways.Desktop.Views.Content.Import;
using EnduranceJudge.Gateways.Desktop.Core.Services.Implementations;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.EnduranceEvents;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager;
using EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participants.Listing;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.Categorizations.Listing;
using EnduranceJudge.Gateways.Desktop.Views.Error;
using Prism.Regions;
using System;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Gateways.Desktop.Services.Implementations
{
    public class NavigationService : NavigationServiceBase, INavigationService
    {
        private readonly IApplicationContext context;
        public NavigationService(IRegionManager regionManager, IApplicationContext context) : base(regionManager)
        {
            this.context = context;
        }

        public void NavigateToImport()
        {
            this.ChangeTo<ImportView>();
            this.ClearRegion(Regions.CONTENT_RIGHT);
        }

        public void NavigateToEvent()
        {
            if (!this.context.IsInitialized)
            {
                this.Error(SELECT_WORK_DIRECTORY);
                return;
            }

            this.ChangeTo<EnduranceEventView>();
            this.ChangeTo<ConfigurationMenuView>(Regions.CONTENT_RIGHT);
        }

        public void NavigateToManager()
        {
            if (!this.context.IsInitialized)
            {
                this.Error(SELECT_WORK_DIRECTORY);
                return;
            }
            this.ChangeTo<ManagerView>();
            this.ChangeTo<ParticipantListView>(Regions.CONTENT_RIGHT);
        }
        public void NavigateToRanking()
        {
            if (!this.context.IsInitialized)
            {
                this.Error(SELECT_WORK_DIRECTORY);
                return;
            }
            this.ChangeTo<CategorizationListView>(Regions.CONTENT_RIGHT);
            this.ClearRegion(Regions.CONTENT_LEFT);
        }

        public void ChangeTo<T>()
            where T : IView
        {
            this.ChangeTo(typeof(T));
        }

        public void ChangeToNewConfiguration<T>(int principalId, int childViewId)
            where T : IView
        {
            var principal = new NavigationParameter(Parameters.PARENT_VIEW_ID, principalId);
            var childView = new NavigationParameter(Parameters.VIEW_ID, childViewId);
            this.ChangeTo(typeof(T), principal, childView);
        }

        public void ChangeToUpdateConfiguration<T>(int id)
            where T : IView
        {
            var parameter = new NavigationParameter(Parameters.DOMAIN_ID, id);
            this.ChangeTo(typeof(T), parameter);
        }

        public void ChangeTo<T>(params NavigationParameter[] parameters)
        {
            this.ChangeTo(typeof(T), parameters);
        }

        public void ChangeTo(Type view, params NavigationParameter[] parameters)
        {
            var navigationParameters = new NavigationParameters();
            foreach (var (key, value) in parameters)
            {
                navigationParameters.Add(key, value);
            }

            this.ChangeTo(Regions.CONTENT_LEFT, view, navigationParameters);
        }

        public void Error(string message)
        {
            var parameter = new NavigationParameter(DesktopConstants.MESSAGE_PARAMETER, message);
            this.ChangeTo<ErrorView>(parameter);
        }
    }
}
