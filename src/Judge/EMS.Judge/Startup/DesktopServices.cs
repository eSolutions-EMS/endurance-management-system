using EMS.Judge.Core.Objects;
using EMS.Judge.Services;
using EMS.Judge.Views.Content.Manager;
using EMS.Judge.Application;
using EMS.Judge.Application.Services;
using EMS.Core;
using EMS.Core.Services;
using EMS.Core.Domain;
using EMS.Core.Localization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prism.Ioc;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EMS.Judge.Startup;

public static class DesktopServices
{
    public static IContainerRegistry AddServices(this IContainerRegistry container)
    {
        new ServiceCollection()
            .AddApplicationServices()
            .AdaptToDesktop(container);

        return container;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assemblies = CoreConstants.Assemblies
            .Concat(LocalizationConstants.Assemblies)
            .Concat(DomainConstants.Assemblies)
            .Concat(ApplicationConstants.Assemblies)
            .Concat(DesktopConstants.Assemblies)
            .ToArray();

        return services
            .AddCore(assemblies)
            .AddDomain(assemblies)
            .AddApplication(assemblies)
            .AddDesktop(assemblies)
            .AddInitializers(assemblies);
    }

    // TODO: Move in Core
    private static IServiceCollection AddInitializers(this IServiceCollection services, Assembly[] assemblies)
        => services
            .Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes =>
                    classes.AssignableTo<IInitializer>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());

    private static IServiceCollection AddDesktop(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddSingleton<IRfidWitness, RfidWitness>();
        services.AddTransient(typeof(IExecutor<>), typeof(Executor<>));
        services.AddJudgeSettings();
        return services;
    }

    private static IServiceCollection AdaptToDesktop(
        this IServiceCollection services,
        IContainerRegistry desktopContainer)
    {
        var adapter = new DesktopContainerAdapter(desktopContainer);
        foreach (var serviceDescriptor in services)
        {
            adapter.Register(serviceDescriptor);
        }

        return services;
    }

    private static void AddJudgeSettings(this IServiceCollection services)
    {
        var contents = File.ReadAllText(DesktopConstants.SETTINGS_FILE);
        var config = JsonSerializer.Deserialize<Settings>(contents, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        services.AddSingleton(config);
        services.AddSingleton<ISettings>(x => x.GetRequiredService<Settings>());
    }
}