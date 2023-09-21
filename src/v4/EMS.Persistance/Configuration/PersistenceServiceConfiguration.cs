using Common.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Persistence.Configuration;

public static class PersistenceServiceConfiguration
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}