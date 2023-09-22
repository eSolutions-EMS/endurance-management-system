using Common.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Domain.Core.Startup;

public static class CoreConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}
