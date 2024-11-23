using Microsoft.Extensions.Logging;
using Not.Injection;
using Not.MAUI.Logging;
using NTS.Judge.MAUI.Server;
using Not.Logging.Builder;
using Not.Storage.Stores.Extensions;
using NTS.Storage.Injection;
using NTS.Judge.Shared;
using NTS.Judge.Blazor;

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
        builder.ConfigureLogging().AddFilesystemLogger<JudgeContext>();

        builder
            .Services.AddJsonFileStore<JudgeContext>()
            .AddStaticOptionsStore<JudgeContext>()
            .AddJudgeBlazor(builder.Configuration)
            .AddInversedDependencies();

        JudgeMauiServer.ConfigurePrentContainer(builder.Services);

        return builder;
    }

    public static IServiceCollection AddInversedDependencies(this IServiceCollection services)
    {
        services
            .AddStorage()
            .AddJudge()
            .GetConventionalAssemblies()
            .RegisterConventionalServices();
        return services;
    }
}
