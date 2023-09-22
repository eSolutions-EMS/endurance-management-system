using Common.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Judge.Server.Startup;

public static class ServerConfiguration
{
    public static IServiceCollection AddServerServices(this IServiceCollection services)
    {
        return services.AddConventionalServices();
    }
}
