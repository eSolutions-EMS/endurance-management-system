using AutoMapper;
using AutoMapper.EquivalencyExpression;
using EnduranceJudge.Application.Contracts.Countries;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Countries;
using EnduranceJudge.Gateways.Persistence.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace EnduranceJudge.Gateways.Persistence.Startup
{
    public static class PersistenceServices
    {
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
                .AddDbContext<EnduranceJudgeDbContext>()
                .Scan(scan => scan
                    .FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.AssignableTo<IDataStore>())
                    .AsSelfWithInterfaces());

        private static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .Scan(scan => scan
                    .FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.AssignableTo(typeof(IQueriesBase<>)))
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime())
                .AddTransient<ICountryQueries, CountryRepository>();

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
    }
}
