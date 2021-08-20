using EnduranceJudge.Core.ConventionalServices;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EnduranceJudge.Core
{
    public static class CoreServices
    {
        public static IServiceCollection AddCore(
            this IServiceCollection services,
            params Assembly[] assemblies)
            => services
                .AddTransientServices(assemblies)
                .AddScopedServices(assemblies)
                .AddSingletonServices(assemblies);

        private static IServiceCollection AddTransientServices(this IServiceCollection services, Assembly[] assemblies)
            => services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IService>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());

        private static IServiceCollection AddScopedServices(this IServiceCollection services, Assembly[] assemblies)
            => services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

        private static IServiceCollection AddSingletonServices(this IServiceCollection services, Assembly[] assemblies)
            => services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());
    }
}
