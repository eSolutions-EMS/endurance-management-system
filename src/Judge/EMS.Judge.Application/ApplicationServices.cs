using Core.Domain.State;
using EMS.Judge.Application.Common;
using EMS.Judge.Application.Models;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.State;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace EMS.Judge.Application;

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

        services.AddState();
        
        return services;
    }

    private static IServiceCollection AddState(this IServiceCollection services)
    {
        services.AddSingleton<IStateSetter, StateModel>();
        services.AddSingleton<IState>(x => x.GetRequiredService<IStateSetter>());
        services.AddTransient<IStateContext, StateContext>();
        return services;
    }
}
