using EnduranceJudge.Domain.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace EnduranceJudge.Domain;

public static class DomainServices
{
    private static readonly Type AGGREGATE_ROOT_TYPE = typeof(IAggregateRoot);

    public static IServiceCollection AddDomain(this IServiceCollection services, Assembly[] assemblies)
    {
        var aggregateRoots = DomainConstants.Assemblies
            .SelectMany(x => x.GetExportedTypes())
            .Where(t => !t.IsInterface && !t.IsAbstract && AGGREGATE_ROOT_TYPE.IsAssignableFrom(t))
            .ToList();
        foreach (var aggregateRoot in aggregateRoots)
        {
            services.AddTransient(aggregateRoot);
        }
        return services;
    }
}