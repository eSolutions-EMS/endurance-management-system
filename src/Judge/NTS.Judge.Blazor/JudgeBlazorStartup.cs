using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.Injection;

namespace NTS.Judge.Blazor;

public static class JudgeBlazorStartup
{
    public static IServiceCollection AddJudgeBlazor(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddLocalization(x => x.ResourcesPath = "Resources/Localization")
            .AddNotBlazor(configuration);
        return services;
    }
}
