using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Desktop.Startup;
using EnduranceJudge.Gateways.Desktop.Views;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using ServiceProvider = EnduranceJudge.Gateways.Desktop.Core.Static.ServiceProvider;

namespace EnduranceJudge.Gateways.Desktop
{
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

            var initializers = aspNetProvider.GetServices<IInitializerInterface>();
            foreach (var initializer in initializers.OrderBy(x => x.RunningOrder))
            {
                initializer.Run(aspNetProvider);
            }
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
            ServiceProvider.Initialize(provider);
            ErrorHandler.Initialize();
        }
    }
}
