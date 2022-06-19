using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain;
using EnduranceJudge.Domain.State;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
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
            
            services.AddSingleton<Context, Context>();
            services.AddSingleton<IContext>(provider => provider.GetRequiredService<Context>());
            services.AddSingleton<IState>(provider => provider.GetRequiredService<Context>().State);
            services.AddInitializers(assemblies);

            var provider = services.BuildServiceProvider();
            
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
