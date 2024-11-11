using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain;

public static class DomainServices
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        Assembly[] assemblies
    )
    {
        return services;
    }
}
