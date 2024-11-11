using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Not.Injection;

public static class ConventionExtensions
{
    const string NOT_PREFIX = "Not.";
    static readonly Type _transientType = typeof(ITransientService);
    static readonly Type _scopedType = typeof(IScopedService);
    static readonly Type _singletonType = typeof(ISingletonService);

    public static (
        IServiceCollection services,
        IEnumerable<Assembly> assemblies
    ) GetConventionalAssemblies(this IServiceCollection services)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        var assemblies = callingAssembly.RecursiveGetReferencedAssemblies([]);
        return (services, assemblies);
    }

    public static IServiceCollection RegisterConventionalServices(
        this (IServiceCollection services, IEnumerable<Assembly> assemblies) values
    )
    {
        var (services, assemblies) = values;
        assemblies = assemblies.Distinct().OrderBy(x => x.FullName).ToList();
        var classes = assemblies
            .SelectMany(x =>
                x.GetTypes()
                    .Where(y => !y.IsInterface && !y.IsAbstract && y.IsConventionalService())
            )
            .ToList();
        foreach (var implementation in classes)
        {
            var interfaces = implementation
                .GetInterfaces()
                .Where(x =>
                    x.IsAssignableFrom(implementation)
                    && x != _transientType
                    && x != _scopedType
                    && x != _singletonType
                )
                .ToList();
            if (implementation.IsSingleton() && implementation.IsTransient())
            {
                throw new Exception(
                    $"Service '{implementation}' cannot both both Singleton and Transient"
                );
            }
            if (implementation.IsScoped() && implementation.IsTransient())
            {
                throw new Exception(
                    $"Service '{implementation}' cannot both both Scoped and Transient"
                );
            }
            if (implementation.IsSingleton() && implementation.IsScoped())
            {
                throw new Exception(
                    $"Service '{implementation}' cannot both both Singleton and Scoped"
                );
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

    static void AddSingleInstance(
        IServiceCollection services,
        IEnumerable<Type> interfaces,
        Type implementation,
        bool isSingleton
    )
    {
        ThrowIfInvalidPolymorphicService(implementation);

        // Commented code uses interface convention and generic type definition in order to select a privary interface
        // to be used in GetRequiredService in order for all services to resolve a single implementation instance.
        // Otherwize if for example 'INotBehind' is selected then GetRequiredService may fail to resove the instance
        // (if more than 1 INotBehind implementations are registered).
        // The code is commented because it is probably unnecessary since we can simply register the implementation as itself
        // and then use that for all interfaces, instead of having to determine a primary interface. Will be deleted later on if this
        // method passes the trial of time
        //var primaryInterface = interfaces
        //    .OrderByDescending(x => x.Name == $"I{implementation.Name}") //
        //    .ThenByDescending(x => x.IsGenericTypeDefinition)
        //    .FirstOrDefault();
        //if (primaryInterface == null)
        //{
        //    var interfaceNames = string.Join(", ", interfaces.Select(x => x.Name));
        //    throw new Exception($"Cannot register service '{implementation.FullName}' as singleton, " +
        //        $"because there is no interface with matching name: '{string.Join(",", interfaces)}'");
        //}
        Add(services, implementation, implementation);
        foreach (var @interface in interfaces)
        {
            if (isSingleton)
            {
                services.AddSingleton(@interface, x => x.GetRequiredService(implementation));
            }
            else
            {
                services.AddScoped(@interface, x => x.GetRequiredService(implementation));
            }
        }
    }

    static void Add(IServiceCollection services, Type @interface, Type implementation)
    {
        var service =
            @interface.IsGenericType && implementation.IsGenericType
                ? @interface.GetGenericTypeDefinition()
                : @interface;

        if (
            service.IsTransient() && (implementation.IsScoped() || implementation.IsSingleton())
            || service.IsScoped() && (implementation.IsTransient() || implementation.IsSingleton())
            || service.IsSingleton() && (implementation.IsTransient() || implementation.IsScoped())
        )
        {
            throw new Exception(
                $"Conflicting lifecycles detected for service '{service.FullName}' and implementation '{implementation.FullName}'"
            );
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
            throw new Exception(
                $"Cannot determine how to register service '{service}'. implementation: '{implementation}''"
            );
        }
    }

    static void ThrowIfInvalidPolymorphicService(Type implementation)
    {
        if (
            implementation.BaseType != null
            && !implementation.BaseType.IsAbstract
            && (
                implementation.IsSingleton() && implementation.BaseType.IsSingleton()
                || implementation.IsScoped() && implementation.BaseType.IsScoped()
            )
        )
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"'{implementation.Name}' and it's parent '{implementation.BaseType.Name}' cannot be "
            );
            sb.AppendLine(
                $"instantiable NonTransient services. Either declare base class as 'abstract' or decouple."
            );
            sb.AppendLine(
                $"This check exists to prevent accidental invokations of the base class instead of the derrived"
            );
            sb.AppendLine(
                $"or to prevent unwanted duplications in case of IEnumerable<T> injection"
            );
            throw new Exception(sb.ToString());
        }
    }

    static Assembly[] RecursiveGetReferencedAssemblies(
        this Assembly assembly,
        List<Assembly> result
    )
    {
        if (result.Any(x => x.FullName == assembly.FullName))
        {
            return result.ToArray();
        }
        result.Add(assembly);
        var ntsPrefix = assembly.FullName!.Split('.').First();
        var references = assembly
            .GetReferencedAssemblies()
            .Where(x =>
                x.FullName!.StartsWith(ntsPrefix)
                || x.FullName!.StartsWith(NOT_PREFIX)
                || x.FullName!.StartsWith("NTS.")
            ) // TODO: remove
            .ToList();
        foreach (var reference in references)
        {
            var innerAssembly = Assembly.Load(reference);
            RecursiveGetReferencedAssemblies(innerAssembly, result);
        }
        return result.ToArray();
    }

    static bool IsConventionalService(this Type type)
    {
        return type.IsTransient() || type.IsScoped() || type.IsSingleton();
    }

    static bool IsTransient(this Type type)
    {
        return type.Name != _transientType.Name && _transientType.IsAssignableFrom(type);
    }

    static bool IsScoped(this Type type)
    {
        return type.Name != _scopedType.Name && _scopedType.IsAssignableFrom(type);
    }

    static bool IsSingleton(this Type type)
    {
        return type.Name != _singletonType.Name && _singletonType.IsAssignableFrom(type);
    }
}
