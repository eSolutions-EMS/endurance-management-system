using Common.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Domain.Startup;

public static class DomainConfiguration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}
