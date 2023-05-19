using Core;
using Core.Application;
using Core.Domain;
using Core.Localization;
using EMS.Judge.Application.Common.Services;
using Core.Services;
using EMS.Judge.Api.Middlewares;
using EMS.Judge.Api.Services;
using EMS.Judge.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace EMS.Judge.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var assemblies = CoreConstants.Assemblies
            .Concat(LocalizationConstants.Assemblies)
            .Concat(DomainConstants.Assemblies)
            .Concat(CoreApplicationConstants.Assemblies)
            .Concat(ApplicationConstants.Assemblies)
            .Concat(ApiConstants.Assemblies)
            .ToArray();
        services
            .AddCore(assemblies)
            .AddApi();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseMiddleware<ErrorLogger>();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}

public static class ApiServices
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(opt => JsonSerializationService.Configure(opt.SerializerSettings));

        services
            .AddTransient<ErrorLogger, ErrorLogger>()
            .AddTransient<IStartlistService, StartlistService>()
            .AddTransient<IWitnessEventService, WitnessEventService>()
            .AddHostedService<NetworkBroadcastService>();

        return services;
    }

    public static IServiceCollection AddInitializers(this IServiceCollection services, Assembly[] assemblies)
        => services
            .Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes =>
                    classes.AssignableTo<IInitializer>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());
}
