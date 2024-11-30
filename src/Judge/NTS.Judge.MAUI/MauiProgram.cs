using Microsoft.Extensions.Logging;
using Not.Filesystem;
using Not.Injection;
using Not.Logging.Builder;
using Not.MAUI.Logging;
using NTS.Judge.Blazor;
using NTS.Judge.MAUI.Server;
using NTS.Judge.Shared;
using NTS.Storage.Injection;

namespace NTS.Judge.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"))
            .ConfigureBlazor();

        return builder.Build();
    }
}

public static class ServiceCollectionExtensions
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

        JudgeMauiServer.ConfigurePrentContainer(builder.Services);

        return builder;
    }

    public static IServiceCollection AddInversedDependencies(this IServiceCollection services)
    {
        services.AddStorage().AddJudge().GetConventionalAssemblies().RegisterConventionalServices();
        return services;
    }
}
