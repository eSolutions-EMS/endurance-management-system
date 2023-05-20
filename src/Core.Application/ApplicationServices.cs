using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application;

public static class ApplicationServices
{
    public static IServiceCollection AddCoreApplication(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        return services;
    }
}
