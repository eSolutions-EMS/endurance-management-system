using Common.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Domain.Setup.Startup;

public static class SetupConfiguration
{
    public static IServiceCollection AddSetupServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}
