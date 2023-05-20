using Core;
using Core.Application;
using Core.Application.Services;
using Core.Domain;
using Core.Localization;
using EMS.Witness.Services;
using EMS.Witness.Platforms.Services;

namespace EMS.Witness;

public static class WitnessConfiguration
{
    public static IServiceCollection AddWitnessServices(this IServiceCollection services)
    {
        var assemblies = CoreConstants.Assemblies
            .Concat(LocalizationConstants.Assemblies)
            .Concat(DomainConstants.Assemblies)
            .Concat(CoreApplicationConstants.Assemblies)
            .Concat(WitnessConstants.Assemblies)
            .ToArray();

        services.AddHttpClient<IApiService, ApiService>(client => client.Timeout = TimeSpan.FromSeconds(5));
        services
            .AddCore(assemblies)
		    .AddTransient<IPermissionsService, PermissionsService>()
            .AddSingleton<ToasterService>()
            .AddSingleton<State>()
            .AddSingleton<IState>(provider => provider.GetRequiredService<State>())
            .AddTransient<IDateService, DateService>();

        return services;
    }
}
