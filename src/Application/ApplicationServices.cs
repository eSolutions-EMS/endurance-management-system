﻿using EnduranceJudge.Application.Core;
using EnduranceJudge.Application.Services;
using EnduranceJudge.Domain.State;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace EnduranceJudge.Application;

public static class ApplicationServices
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueries<>)))
            .AsSelfWithInterfaces()
            .WithTransientLifetime());
        
        // TODO: get that working
        // services.AddHttpClient();
        // var kur = services.FirstOrDefault(x => x.ServiceType == typeof(IServiceScopeFactory));

        services.AddSingleton<IStateSetter, State>();
        services.AddTransient<IState>(x => x.GetRequiredService<IStateSetter>());
        return services;
    }
}