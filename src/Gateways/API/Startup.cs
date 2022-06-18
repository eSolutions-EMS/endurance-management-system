using EnduranceJudge.Core;
using EnduranceJudge.Domain;
using EnduranceJudge.Gateways.Persistence;
using EnduranceJudge.Gateways.Persistence.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
                .Concat(PersistenceConstants.Assemblies)
                .Concat(ApiConstants.Assemblies)
                .ToArray();

            services
                .AddCore(assemblies)
                .AddDomain(assemblies)
                .AddPersistence(assemblies)
                .AddApi(assemblies);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
    
    public static class ApiServices
    {
        public static IServiceCollection AddApi(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });
            return services;
        }
    }
}
