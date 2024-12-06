using Microsoft.Extensions.Logging;
using Not.MAUI.Logging;
using NTS.Judge.Blazor;
using NTS.Judge.Shared;
using NTS.Storage.Injection;
using Not.Filesystem;
using Not.Logging.Builder;
using Not.Injection;

namespace NTS.Judge.MAUI.Injection;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder ConfigureBlazor(this MauiAppBuilder builder)
    {
        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        FileContextHelper.SetDebugRootDirectory("nts");
        builder.ConfigureLogging().AddFilesystemLogger();

        builder.Services
            .AddJudgeBlazor(builder.Configuration)
            .AddInversedDependencies();

        return builder;
    }

    public static IServiceCollection AddInversedDependencies(this IServiceCollection services)
    {
        services.AddStorage().AddJudge().GetConventionalAssemblies().RegisterConventionalServices();
        return services;
    }
}
