using AutoMapper;
using AutoMapper.EquivalencyExpression;
using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using EnduranceJudge.Domain.Aggregates.Event.Participants;
using EnduranceJudge.Domain.Aggregates.Event.Personnels;
using EnduranceJudge.Domain.Aggregates.Event.Phases;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Gateways.Persistence.Contracts.Repositories;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Athletes;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Entities.Horses;
using EnduranceJudge.Gateways.Persistence.Entities.Participants;
using EnduranceJudge.Gateways.Persistence.Entities.Personnel;
using EnduranceJudge.Gateways.Persistence.Entities.Phases;
using EnduranceJudge.Gateways.Persistence.Entities.PhasesForCategories;
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
                .AddDbContext<EnduranceJudgeDbContext>();

        private static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddConventional()
                .AddCommonRepositories()
                .AddImportRepositories()
                .AddEventRepositories()
                .AddManagerRepositories()
                .AddRankingRepositories();

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

        private static IServiceCollection AddConventional(this IServiceCollection services)
            => services
                .Scan(scan => scan
                    .FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.AssignableTo(typeof(IQueries<>)))
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime());

        private static IServiceCollection AddCommonRepositories(this IServiceCollection services)
            => services
                .AddTransient<IQueries<Athlete>, Repository<AthleteEntity, Athlete>>()
                .AddTransient<ICommands<Athlete>, Repository<AthleteEntity, Athlete>>()
                .AddTransient<IQueries<Horse>, Repository<HorseEntity, Horse>>()
                .AddTransient<ICommands<Horse>, Repository<HorseEntity, Horse>>()
                .AddTransient<ICountryQueries, CountryRepository>();

        private static IServiceCollection AddEventRepositories(this IServiceCollection services)
            => services
                .AddTransient<IQueries<Competition>, Repository<CompetitionEntity, Competition>>()
                .AddTransient<ICommands<Competition>, Repository<CompetitionEntity, Competition>>()
                .AddTransient<IQueries<EnduranceEvent>, Repository<EnduranceEventEntity, EnduranceEvent>>()
                .AddTransient<ICommands<EnduranceEvent>, Repository<EnduranceEventEntity, EnduranceEvent>>()
                .AddTransient<IQueries<Participant>, Repository<ParticipantEntity, Participant>>()
                .AddTransient<ICommands<Participant>, Repository<ParticipantEntity, Participant>>()
                .AddTransient<IQueries<Personnel>, Repository<PersonnelEntity, Personnel>>()
                .AddTransient<ICommands<Personnel>, Repository<PersonnelEntity, Personnel>>()
                .AddTransient<IQueries<Phase>, Repository<PhaseEntity, Phase>>()
                .AddTransient<ICommands<Phase>, Repository<PhaseEntity, Phase>>()
                .AddTransient<
                    IQueries<PhaseForCategory>,
                    Repository<PhaseForCategoryEntity, PhaseForCategory>>()
                .AddTransient<ICommands<
                    PhaseForCategory>,
                    Repository<PhaseForCategoryEntity, PhaseForCategory>>();

        private static IServiceCollection AddImportRepositories(this IServiceCollection services)
            => services
                .AddTransient<
                    IQueries<Domain.Aggregates.Import.Competitions.Competition>,
                    Repository<CompetitionEntity, Domain.Aggregates.Import.Competitions.Competition>>()
                .AddTransient<
                    ICommands<Domain.Aggregates.Import.Competitions.Competition>,
                    Repository<CompetitionEntity, Domain.Aggregates.Import.Competitions.Competition>>()
                .AddTransient<
                    IQueries<Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent>,
                    Repository<EnduranceEventEntity, Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent>>()
                .AddTransient<
                    ICommands<Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent>,
                    Repository<EnduranceEventEntity, Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent>>()
                .AddTransient<
                    IQueries<Domain.Aggregates.Import.Participants.Participant>,
                    Repository<ParticipantEntity, Domain.Aggregates.Import.Participants.Participant>>()
                .AddTransient<
                    ICommands<Domain.Aggregates.Import.Participants.Participant>,
                    Repository<ParticipantEntity, Domain.Aggregates.Import.Participants.Participant>>();

        private static IServiceCollection AddManagerRepositories(this IServiceCollection services)
            => services;

        private static IServiceCollection AddRankingRepositories(this IServiceCollection services)
            => services;
    }
}
