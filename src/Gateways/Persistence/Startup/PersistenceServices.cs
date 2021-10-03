using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Persistence.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnduranceJudge.Gateways.Persistence.Startup
{
    public static class PersistenceServices
    {
        private static readonly Type AggregateRootEntityBaseType = typeof(AggregateRootEntityBase);
        private static readonly Type IQueriesType = typeof(IQueries<>);
        private static readonly Type ICommandsType = typeof(ICommands<>);

        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddDataProtection();

            return services
                .AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var entityTypes = ReflectionUtilities
                .GetExportedTypes(typeof(PersistenceServices).Assembly)
                .Where(AggregateRootEntityBaseType.IsAssignableFrom)
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var entity = Activator.CreateInstance(entityType) as AggregateRootEntityBase;
                var domainTypes = entity!.DomainTypes;
                if (domainTypes == null)
                {
                    continue;
                }
            }

            return services;
        }
    }
}
