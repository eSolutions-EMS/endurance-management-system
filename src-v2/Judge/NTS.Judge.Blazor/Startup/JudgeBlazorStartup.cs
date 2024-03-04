using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace NTS.Judge.Blazor.Startup;

public static class JudgeBlazorStartup
{
    public static IServiceCollection AddJudgeBlazor(this IServiceCollection services)
    {
        services
            .AddLocalization(x => x.ResourcesPath = "Resources/Localization")
            .AddMudServices();
        return services;
    }
}
