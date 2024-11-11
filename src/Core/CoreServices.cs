using System.Reflection;
using AutoMapper;
using Core.ConventionalServices;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreServices
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        return services
            .AddMapping(assemblies)
            .AddTransientServices(assemblies)
            .AddScopedServices(assemblies)
            .AddSingletonServices(assemblies);
    }

    private static IServiceCollection AddTransientServices(
        this IServiceCollection services,
        Assembly[] assemblies
    ) =>
        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<ITransientService>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );

    private static IServiceCollection AddScopedServices(
        this IServiceCollection services,
        Assembly[] assemblies
    ) =>
        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
        );

    private static IServiceCollection AddSingletonServices(
        this IServiceCollection services,
        Assembly[] assemblies
    ) =>
        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime()
        );

    private static IServiceCollection AddMapping(
        this IServiceCollection services,
        Assembly[] assemblies
    ) =>
        services.AddAutoMapper(
            configuration =>
            {
                configuration.DisableConstructorMapping();
            },
            assemblies
        );
}
