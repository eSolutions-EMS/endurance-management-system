using Endurance.Judge.Gateways.API.Middlewares;
using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace Endurance.Judge.Gateways.API
{
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
                .Concat(ApiConstants.Assemblies)
                .ToArray();

            services
                .AddCore(assemblies)
                .AddApi(assemblies)
                .AddDomain(assemblies);
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

            // TODO: extract this logic
            var initializers = provider.GetServices<IInitializer>();
            foreach (var initializer in initializers.OrderBy(x => x.RunningOrder))
            {
                initializer.Run();
            }
        }
    }
    
    public static class ApiServices
    {
        public static IServiceCollection AddApi(this IServiceCollection services, Assembly[] assemblies)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(opt => JsonSerializationService.Configure(opt.SerializerSettings));

            services.AddTransient<ErrorLogger, ErrorLogger>();
            
            services.AddSingleton<ApiContext, ApiContext>();
            services.AddSingleton<IApiContext>(provider => provider.GetRequiredService<ApiContext>());
            services.AddSingleton<IStateContext>(provider => provider.GetRequiredService<ApiContext>());
            services.AddInitializers(assemblies);

            return services;
        }
        
        private static IServiceCollection AddInitializers(this IServiceCollection services, Assembly[] assemblies)
            => services
                .Scan(scan => scan
                    .FromAssemblies(assemblies)
                    .AddClasses(classes =>
                        classes.AssignableTo<IInitializer>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime());
    }
}
