using EMS.Judge.Application.Core.Services;
using EMS.Judge.Application.Models;
using EMS.Core;
using EMS.Core.Services;
using EMS.Core.Domain;
using EMS.Core.Domain.State;
using EMS.Judge.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace EMS.Judge.Api
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
                .Concat(ApiConstants.Assemblies)
                .ToArray();

            services
                .AddCore(assemblies)
                .AddApi(assemblies)
                .AddInitializers(assemblies);
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
}
