using Microsoft.Extensions.DependencyInjection;
using Not.Domain.Ports;

namespace Not.Storage.StaticOptions;

public static class Extensions
{
    public static IServiceCollection AddStaticOptionsStore<T>(this IServiceCollection services)
        where T : class, IStaticOptionsConfiguration, new()
    {
        services.AddSingleton<IStaticOptionsConfiguration, T>();
        services.AddSingleton(typeof(IStaticOptionsProvider<>), typeof(JsonStaticOptionsStore<>));
        return services;
    }
}
