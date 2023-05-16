using EMS.Judge.Core;
using EMS.Judge.Views;
using EMS.Core.Services;
using EMS.Core.Utilities;
using EMS.Judge.Startup;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace EMS.Judge;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry container)
        => container.AddServices();

    protected override Window CreateShell()
    {
        this.InitializeApplication();

        return this.Container.Resolve<ShellWindow>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        base.ConfigureModuleCatalog(moduleCatalog);

        var moduleDescriptors = ReflectionUtilities.GetDescriptors<ModuleBase>(Assembly.GetExecutingAssembly());
        foreach (var descriptor in moduleDescriptors)
        {
            moduleCatalog.AddModule(descriptor.Type);
        }
    }

    private void InitializeApplication()
    {
        var aspNetProvider = this.Container.Resolve<IServiceProvider>();
        InitializeStaticServices(aspNetProvider);

        var initializers = aspNetProvider.GetServices<IInitializer>();
        foreach (var initializer in initializers.OrderBy(x => x.RunningOrder))
        {
            initializer.Run();
        }
        Console.WriteLine("================================================");
        Console.WriteLine("=        ENDURANCE JUDGE UI running             ");
        Console.WriteLine("================================================");
    }

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();

        ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
        {
            var viewNamespace = viewType.Namespace;
            var assembly = viewType.Assembly;

            var typesInNamespace = assembly
                .GetExportedTypes()
                .Where(t => t.Namespace == viewNamespace)
                .ToList();

            var viewModelType = typesInNamespace.FirstOrDefault(t =>
                typeof(ViewModelBase).IsAssignableFrom(t));

            return viewModelType;
        });
    }

    private static void InitializeStaticServices(IServiceProvider provider)
    {
        StaticProvider.Initialize(provider);
    }
}
