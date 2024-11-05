using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class ApplicationServices
{
    public static IServiceCollection AddCoreApplication(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        return services;
    }
}
