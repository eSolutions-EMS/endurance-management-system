﻿using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Not.Injection;

public static class InectionExtensions
{
    const string NOT_PREFIX = "Not.";
    static readonly Type _transientType = typeof(ITransient);
    static readonly Type _scopedType = typeof(IScoped);
    static readonly Type _singletonType = typeof(ISingleton);

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
            RegisterImplemenationByConvention(implementation, services);
        }
        return services;
    }

    static void RegisterImplemenationByConvention(Type implementation, IServiceCollection services)
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

        PreventInvalidConventialService(implementation);

        if (interfaces.Any(x => x.IsSingleton()))
        {
            AddAsSelfWithInterfaces(
                services,
                interfaces,
                implementation,
                ServiceLifetime.Singleton
            );
            return;
        }
        if (interfaces.Any(x => x.IsScoped()))
        {
            AddAsSelfWithInterfaces(services, interfaces, implementation, ServiceLifetime.Scoped);
            return;
        }
        foreach (var i in interfaces)
        {
            Add(services, i, implementation);
        }
    }

    static void PreventInvalidConventialService(Type implementation)
    {
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
    }

    static void AddAsSelfWithInterfaces(
        IServiceCollection services,
        IEnumerable<Type> interfaces,
        Type implementation,
        ServiceLifetime lifetime
    )
    {
        PreventInvalidPolymorphicService(implementation);

        // Register as self and use self to fetch the instace for all interfaces
        Add(services, implementation, implementation);
        foreach (var @interface in interfaces)
        {
            var descriptor = new ServiceDescriptor(
                @interface,
                x => x.GetRequiredService(implementation),
                lifetime
            );
            services.Add(descriptor);
        }
    }

    static void Add(IServiceCollection services, Type @interface, Type implementation)
    {
        var service =
            @interface.IsGenericType && implementation.IsGenericType
                ? @interface.GetGenericTypeDefinition()
                : @interface;

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

    static void PreventInvalidPolymorphicService(Type implementation)
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