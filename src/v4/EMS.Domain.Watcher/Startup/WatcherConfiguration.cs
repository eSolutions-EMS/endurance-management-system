using Common.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Domain.Watcher.Startup;

public static class WatcherConfiguration
{
    public static IServiceCollection AddWatcherServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}
