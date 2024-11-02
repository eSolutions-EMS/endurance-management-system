using System;
using System.Collections.Generic;
using Core.Models;
using Core.Utilities;
using EMS.Judge.Views.Dialogs.Confirmation;
using EMS.Judge.Views.Dialogs.Message;
using EMS.Judge.Views.Dialogs.Startlists;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace EMS.Judge.Common;

public class ModuleBase : IModule
{
    private static readonly Type ModuleType = typeof(EntryModule);

    public virtual void RegisterTypes(IContainerRegistry containerRegistry)
    {
        this.RegisterViewsForNavigation(containerRegistry);
        containerRegistry.RegisterDialog<MessageDialog>();
        containerRegistry.RegisterDialog<ConfirmationDialog>();
        containerRegistry.RegisterDialog<StartlistDialog>();
    }

    public virtual void OnInitialized(IContainerProvider containerProvider)
    {
        this.RegisterViewsInRegions(containerProvider);
    }

    private void RegisterViewsForNavigation(IContainerRegistry containerRegistry)
    {
        var descriptors = this.GetViewDescriptors();

        foreach (var descriptor in descriptors)
        {
            containerRegistry.RegisterForNavigation(descriptor.Type, descriptor.Type.Name);
        }
    }

    private void RegisterViewsInRegions(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();
        var viewsDescriptors = this.GetViewDescriptors();

        foreach (var descriptor in viewsDescriptors)
        {
            regionManager.RegisterViewWithRegion(descriptor.Instance.RegionName, descriptor.Type);
        }
    }

    protected IEnumerable<TypeDescriptor<IView>> GetViewDescriptors()
    {
        return ReflectionUtilities.GetDescriptors<IView>(ModuleType.Assembly);
    }
}
