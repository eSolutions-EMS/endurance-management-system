using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;

namespace Not.Injection;

public static class ConventionExtensions
{
    private const string NOT_PREFIX = "Not.";
    private static readonly Type TransientType = typeof(ITransientService);
    private static readonly Type ScopedType = typeof(IScopedService);
    private static readonly Type SingletonType = typeof(ISingletonService);

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
            .OrderBy(x => x.FullName)
            .ToList();
        var classes = assemblies
            .SelectMany(x => x
                .GetTypes()
                .Where(y => !y.IsInterface && !y.IsAbstract && y.IsConventionalService()))
            .ToList();
        foreach (var implementation in classes)
        {
            var interfaces = implementation.GetInterfaces()
                .Where(x => x.IsAssignableFrom(implementation) && x != TransientType && x != ScopedType && x != SingletonType)
                .ToList();
            if (implementation.IsSingleton() && implementation.IsTransient())
            {
                throw new Exception($"Service '{implementation}' cannot both both Singleton and Transient");
            }
            if (implementation.IsScoped() && implementation.IsTransient())
            {
                throw new Exception($"Service '{implementation}' cannot both both Scoped and Transient");
            }
            if (implementation.IsSingleton() && implementation.IsScoped())
            {
                throw new Exception($"Service '{implementation}' cannot both both Singleton and Scoped");
            }

            if (interfaces.Any(x => x.IsSingleton()))
            {
                AddSingleInstance(services, interfaces, implementation, isSingleton: true);
                continue;
            }
            
            if (interfaces.Any(x => x.IsScoped()))
            {
                AddSingleInstance(services, interfaces, implementation, isSingleton: false);
                continue;
            }
            
            foreach (var i in interfaces)
            {
                Add(services, i, implementation); 
            }
        }
        return services;
    }

    private static void AddSingleInstance(IServiceCollection services, IEnumerable<Type> interfaces, Type implementation, bool isSingleton)
    {
        ThrowIfInvalidPolymorphicService(implementation);

        var first = interfaces.First();
        Add(services, first, implementation);
        foreach (var @interface in interfaces.Skip(1))
        {
            if (isSingleton)
            {
                services.AddSingleton(@interface, x => x.GetRequiredService(first));
            }
            else
            {
                services.AddScoped(@interface, x => x.GetRequiredService(first));
            }
        }
    }

    private static void Add(IServiceCollection services, Type @interface, Type implementation)
    {
        var service = @interface.IsGenericType && implementation.IsGenericType
            ? @interface.GetGenericTypeDefinition()
            : @interface;

        if (service.IsTransient() && (implementation.IsScoped() || implementation.IsSingleton())
            || service.IsScoped() && (implementation.IsTransient() || implementation.IsSingleton())
            || service.IsSingleton() && (implementation.IsTransient() || implementation.IsScoped()))
        {
            throw new Exception($"Conflicting lifecycles detected for service '{service.FullName}' and implementation '{implementation.FullName}'");
        }

        if (implementation.IsTransient())
        {
            services.AddTransient(service, implementation);
        }
        else if (implementation.IsScoped())
        {
            services.AddScoped(service, implementation);
        }
        else if (implementation.IsSingleton())
        {
            services.AddSingleton(service, implementation);
        }
        else
        {
            throw new Exception($"Cannot determine how to register service '{service}'. implementation: '{implementation}''");
        }
    }

    private static void ThrowIfInvalidPolymorphicService(Type implementation)
    {
        if (implementation.BaseType != null &&
            !implementation.BaseType.IsAbstract &&
            (implementation.IsSingleton() && implementation.BaseType.IsSingleton()
            || implementation.IsScoped() && implementation.BaseType.IsScoped()))
        {
            var sb = new StringBuilder();
            sb.AppendLine($"'{implementation.Name}' and it's parent '{implementation.BaseType.Name}' cannot be ");
            sb.AppendLine($"instantiable NonTransient services. Either declare base class as 'abstract' or decouple.");
            sb.AppendLine($"This check exists to prevent accidental invokations of the base class instead of the derrived");
            sb.AppendLine($"or to prevent unwanted duplications in case of IEnumerable<T> injection");
            throw new Exception(sb.ToString());
        }
    }

    private static Assembly[] RecursiveGetReferencedAssemblies(this Assembly assembly, List<Assembly> result)
    {
        if (result.Any(x => x.FullName == assembly.FullName))
        {
            return result.ToArray();
        }
        result.Add(assembly);
        var ntsPrefix = assembly.FullName!.Split('.').First();
        var references = assembly
            .GetReferencedAssemblies()
            .Where(x => x.FullName!.StartsWith(ntsPrefix) || x.FullName!.StartsWith(NOT_PREFIX) || x.FullName!.StartsWith("NTS.")) // TODO: remove
            .ToList();
        foreach (var reference in references)
        {
            var innerAssembly = Assembly.Load(reference);
            RecursiveGetReferencedAssemblies(innerAssembly, result);
        }
        return result.ToArray();
    }
     
    private static bool IsConventionalService(this Type type)
        => type.IsTransient() || type.IsScoped() || type.IsSingleton();

    private static bool IsTransient(this Type type)
        => type.Name != TransientType.Name && TransientType.IsAssignableFrom(type);

    private static bool IsScoped(this Type type)
        => type.Name != ScopedType.Name && ScopedType.IsAssignableFrom(type);

    private static bool IsSingleton(this Type type)
        => type.Name != SingletonType.Name && SingletonType.IsAssignableFrom(type);
}
