using EMS.Judge.Core;
using EMS.Judge.Core.Services.Implementations;
using EMS.Judge.Views.Content.Configuration.ConfigurationMenu;
using EMS.Judge.Views.Content.Configuration.Roots.Events;
using EMS.Judge.Views.Content.Hardware;
using EMS.Judge.Views.Content.Import;
using EMS.Judge.Views.Content.Manager;
using EMS.Judge.Views.Content.Ranking;
using EMS.Core.Application.Core.Exceptions;
using Prism.Regions;
using System;
using static EMS.Judge.DesktopConstants;
using static EMS.Core.Localization.Strings;
using NavigationParameters = Prism.Regions.NavigationParameters;

namespace EMS.Judge.Services.Implementations;

public class NavigationService : NavigationServiceBase, INavigationService
{
    private readonly IStatelessExecutor executor;
    private readonly IApplicationContext context;
    public NavigationService(
        IStatelessExecutor executor,
        IRegionManager regionManager,
        IApplicationContext context) : base(regionManager)
    {
        this.executor = executor;
        this.context = context;
    }

    public void NavigateToImport()
    {
        this.executor.Execute(() =>
        {
            this.ChangeTo<ImportView>();
            this.ChangeTo<ConfigurationMenuView>(Regions.CONTENT_RIGHT);
        });
    }

    public void NavigateToEvent()
    {
        this.executor.Execute(() =>
        {
            if (!this.context.IsInitialized)
            {
                throw new AppException(SELECT_WORK_DIRECTORY_MESSAGE);
            }
            this.ChangeTo<EnduranceEventView>();
            this.ChangeTo<ConfigurationMenuView>(Regions.CONTENT_RIGHT);
        });
    }

    public void NavigateToManager()
    {
        this.executor.Execute(() =>
        {
            if (!this.context.IsInitialized)
            {
                throw new AppException(SELECT_WORK_DIRECTORY_MESSAGE);
            }
            this.ChangeTo<ManagerView>();
        });
    }
    public void NavigateToRanking()
    {
        this.executor.Execute(() =>
        {
            if (!this.context.IsInitialized)
            {
                throw new AppException(SELECT_WORK_DIRECTORY_MESSAGE);
            }
            this.ChangeTo<RankingView>(Regions.CONTENT_LEFT);
        });
    }
    public void NavigateToHardware()
    {
        this.executor.Execute(() => this.ChangeTo<HardwareView>(Regions.CONTENT_LEFT));
    }

    public void ChangeTo<T>()
        where T : IView
    {
        this.executor.Execute(() =>
        {
            this.ChangeTo(typeof(T));
        });
    }

    public void ChangeToNewConfiguration<T>(int principalId, int childViewId)
        where T : IView
    {
        this.executor.Execute(() =>
        {
            var principal = new NavigationParameter(DesktopConstants.NavigationParametersKeys.PARENT_VIEW_ID, principalId);
            var childView = new NavigationParameter(DesktopConstants.NavigationParametersKeys.VIEW_ID, childViewId);
            this.ChangeTo(typeof(T), principal, childView);
        });
    }

    public void ChangeToUpdateConfiguration<T>(int id)
        where T : IView
    {
        this.executor.Execute(() =>
        {
            var parameter = new NavigationParameter(DesktopConstants.NavigationParametersKeys.DOMAIN_ID, id);
            this.ChangeTo(typeof(T), parameter);
        });
    }

    public void ChangeTo<T>(params NavigationParameter[] parameters)
    {
        this.executor.Execute(() =>
        {
            this.ChangeTo(typeof(T), parameters);
        });
    }

    public void ChangeTo(Type view, params NavigationParameter[] parameters)
    {
        this.executor.Execute(() =>
        {
            var navigationParameters = new NavigationParameters();
            foreach (var (key, value) in parameters)
            {
                navigationParameters.Add(key, value);
            }
            this.ChangeTo(Regions.CONTENT_LEFT, view, navigationParameters);
        });
    }
}
