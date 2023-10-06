using Common.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Conventions;

public static class ConventionExtensions
{
    private static Type TransientType = typeof(ITransientService);
    private static Type ScopedType = typeof(IScopedService);
    private static Type SingletonType = typeof(ISingletonService);

    public static (IServiceCollection services, IEnumerable<Assembly> assemblies) GetConventionalAssemblies(this IServiceCollection services)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        var assemblies = callingAssembly.RecursiveGetReferencedAssemblies(new List<Assembly>());
        return (services, assemblies);
    }

    public static IServiceCollection RegisterConventionalServices(this (IServiceCollection services, IEnumerable<Assembly> assemblies) values)
    {
        var (services, assemblies) = values;
        assemblies = assemblies
            .Distinct() 
            .ToList();
        var classes = assemblies
            .SelectMany(x => x
                .GetTypes()
                .Where(y => !y.IsInterface && !y.IsAbstract && y.IsConventionalService()))
            .ToList();
        foreach (var c in classes)
        {
            var interfaces = c.GetInterfaces()
                .Where(x => x.IsAssignableFrom(c))
                .ToList();
            foreach (var i in interfaces)
            {
                var service = i.IsGenericType ? i.GetGenericTypeDefinition() : i;
                if (service.IsTransient())
                {
                    services.AddTransient(service, c);
                }
                else if (service.IsScoped())
                {
                    services.AddScoped(service, c);
                }
                else if (service.IsSingleton())
                {
                    services.AddSingleton(service, c);
                }
            }
        }

        return services;
    }
    private static Assembly[] RecursiveGetReferencedAssemblies(this Assembly assembly, List<Assembly> result)
    {
        if (result.Any(x => x.FullName == assembly.FullName))
        {
            return result.ToArray();
        }
        result.Add(assembly);
        var namespacePrefix = assembly.FullName!.Split('.').First();
        var references = assembly
            .GetReferencedAssemblies()
            .Where(x => x.FullName!.StartsWith(namespacePrefix) || x.FullName!.StartsWith("Common"))
            .ToList();
        foreach (var reference in references)
        {
            var innerAssembly = Assembly.Load(reference);
            RecursiveGetReferencedAssemblies(innerAssembly, result);
        }
        return result.ToArray();
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
        => type.Name != TransientType.Name && TransientType.IsAssignableFrom(type);

    private static bool IsScoped(this Type type)
        => type.Name != ScopedType.Name && ScopedType.IsAssignableFrom(type);

    private static bool IsSingleton(this Type type)
        => type.Name != SingletonType.Name && SingletonType.IsAssignableFrom(type);
}
