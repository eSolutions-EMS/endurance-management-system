using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.Mud.Extensions;

namespace NTS.Judge.Blazor.Startup;

public static class JudgeBlazorStartup
{
    public static IServiceCollection AddJudgeBlazor(this IServiceCollection services)
    {
        services
            .AddLocalization(x => x.ResourcesPath = "Resources/Localization")
            .AddNotMudBlazor();
        return services;
    }
}
