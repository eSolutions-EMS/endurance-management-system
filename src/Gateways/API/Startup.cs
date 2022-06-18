using AutoMapper;
using Endurance.Judge.Gateways.API.Jobs;
using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core;
using EnduranceJudge.Core.Mappings;
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
                .AddDomain(assemblies)
                .AddApi(assemblies);
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
            
            StaticProvider.Initialize(provider);
        }
    }
    
    public static class ApiServices
    {
        public static IServiceCollection AddApi(this IServiceCollection services, Assembly[] assemblies)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(opt => JsonSerializationService.Configure(opt.SerializerSettings));
            
            services.AddHostedService<StateUpdateJob>();
            services.AddSingleton<Context, Context>();
            services.AddSingleton<IReadonlyContext>(provider => provider.GetRequiredService<Context>());
            services.AddTransient<IState>(provider => provider.GetRequiredService<Context>().State);
            
            return services;
        }
    }
}
