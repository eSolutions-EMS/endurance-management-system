using Microsoft.Extensions.DependencyInjection;
using Not.Domain.Ports;
using Not.Storage.Stores.Files;
using Not.Storage.Stores.Files.Ports;
using Not.Storage.Stores.StaticOptions.Adapters;
using Not.Storage.Stores.StaticOptions.Ports;

namespace Not.Storage.Stores.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJsonFileStore(this IServiceCollection services, Action<FileContext> configure)
    {
        var factory = ConfigureFileContext(configure);
        services.AddSingleton<IFileStorageConfiguration, FileContext>(factory);
        services.AddSingleton(typeof(IStore<>), typeof(LockingJsonFileStore<>));
        return services;
    }

    public static IServiceCollection AddStaticOptionsStore(this IServiceCollection services, Action<FileContext> configure)
    {
        var factory = ConfigureFileContext(configure);
        services.AddSingleton<IStaticOptionsConfiguration, FileContext>(factory);
        services.AddSingleton(
            typeof(IStaticOptionsProvider<>),
            typeof(JsonStaticOptionsProvider<>)
        );
        return services;
    }

    static Func<IServiceProvider, FileContext> ConfigureFileContext(Action<FileContext> configure)
    {
        var context = new FileContext();
        configure(context);
        context.Validate();
        return _ => context;
    }
}
