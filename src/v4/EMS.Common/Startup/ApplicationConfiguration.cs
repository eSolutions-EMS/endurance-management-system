using Common.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Application.Startup;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}
