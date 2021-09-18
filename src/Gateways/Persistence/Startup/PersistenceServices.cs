using AutoMapper;
using AutoMapper.EquivalencyExpression;
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
        private static readonly Type EntityBaseType = typeof(EntityBase);
        private static readonly Type IQueriesType = typeof(IQueries<>);
        private static readonly Type ICommandsType = typeof(ICommands<>);
        private static readonly Type RepositoryType = typeof(Repository<,>);

        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddDataProtection();

            return services
                .AddDatabase()
                .AddMapping(assemblies)
                .AddRepositories();
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
            => services
                .AddDbContext<EnduranceJudgeDbContext>();

        private static IServiceCollection AddMapping(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
            => services.AddAutoMapper(
                configuration =>
                {
                    configuration.AddCollectionMappers();
                    configuration.UseEntityFrameworkCoreModel<EnduranceJudgeDbContext>();
                    configuration.DisableConstructorMapping();
                },
                assemblies);

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var entityTypes = ReflectionUtilities
                .GetExportedTypes(typeof(PersistenceServices).Assembly)
                .Where(EntityBaseType.IsAssignableFrom)
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var entity = Activator.CreateInstance(entityType) as EntityBase;
                var domainTypes = entity!.DomainTypes;
                if (domainTypes == null)
                {
                    continue;
                }

                foreach (var domainType in domainTypes)
                {
                    var (queries, commands, repository) = CreateTypes(entityType, domainType);

                    services
                        .AddTransient(queries, repository)
                        .AddTransient(commands, repository);
                }
            }

            return services;
        }

        private static (Type queries, Type commands, Type repository) CreateTypes(Type entityType, Type domainType)
        {
            var commands = ICommandsType.MakeGenericType(domainType);
            var queries = IQueriesType.MakeGenericType(domainType);
            var repository = RepositoryType.MakeGenericType(entityType, domainType);
            return (queries, commands, repository);
        }
    }
}
