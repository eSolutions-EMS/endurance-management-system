using Microsoft.Extensions.DependencyInjection;
using Not.Domain.Ports;
using Not.Storage.Stores.Files;
using Not.Storage.Stores.Files.Ports;
using Not.Storage.Stores.StaticOptions.Adapters;
using Not.Storage.Stores.StaticOptions.Ports;

namespace Not.Storage.Stores.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJsonFileStore<T>(this IServiceCollection services)
        where T : class, IFileStorageConfiguration, new()
    {
        services.AddSingleton<IFileStorageConfiguration, T>();
        services.AddSingleton(typeof(IStore<>), typeof(LockingJsonFileStore<>));
        return services;
    }

    public static IServiceCollection AddStaticOptionsStore<T>(this IServiceCollection services)
        where T : class, IStaticOptionsConfiguration, new()
    {
        services.AddSingleton<IStaticOptionsConfiguration, T>();
        services.AddSingleton(typeof(IStaticOptionsProvider<>), typeof(JsonStaticOptionsProvider<>));
        return services;
    }
}
