using Common.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Startup;

public static class ServiceCollectionExtensions
{
    private static Type TransientType = typeof(ITransientService);
    private static Type ScopedType = typeof(IScopedService);
    private static Type SingletonType = typeof(ISingletonService);

    public static IServiceCollection AddConventionalServices(this IServiceCollection services)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        var referencedAssemblies = callingAssembly.GetReferencedAssemblies().Select(Assembly.Load);
        var assemblies = referencedAssemblies.Concat(new List<Assembly> { callingAssembly });

        var interfaces = assemblies.SelectMany(x => x
            .GetTypes()
            .Where(y => y.IsInterface && y.IsConventionalService()));
        var classes = assemblies.SelectMany(x => x
            .GetTypes()
            .Where(y => !y.IsInterface && !y.IsAbstract));
        foreach (var i in interfaces)
        {
            foreach (var c in classes)
            {
                if (i.IsAssignableFrom(c))
                {
                    if (i.IsTransient())
                    {
                        services.AddTransient(i, c);
                    }
                    if (i.IsScoped())
                    {
                        services.AddScoped(i, c);
                    }
                    if (i.IsSingleton())
                    {
                        services.AddSingleton(i, c);
                    }
                }
            }
        }

        return services;
    }

    private static bool IsConventionalService(this Type type)
    {
        var isTransient = type.IsTransient();
        var isScoped = type.IsScoped();
        var isSingleton = type.IsSingleton();

        if (isTransient && isScoped || isTransient && isSingleton || isScoped && isSingleton)
        {
            throw new Exception($"'{type.FullName}' can be ONLY one of " +
                $"'{nameof(ITransientService)}', '{nameof(IScopedService)}', '{nameof(ISingletonService)}' ");
        }

        return isTransient || isScoped || isSingleton;
    }

    private static bool IsTransient(this Type type)
        => TransientType.IsAssignableFrom(type);

    private static bool IsScoped(this Type type)
        => ScopedType.IsAssignableFrom(type);

    private static bool IsSingleton(this Type type)
        => SingletonType.IsAssignableFrom(type);
}
