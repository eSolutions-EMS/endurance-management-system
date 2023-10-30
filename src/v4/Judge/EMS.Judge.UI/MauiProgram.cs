using Common.Conventions;
using EMS.Judge.Setup.Events;
using EMS.Persistence.Startup;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace EMS.Judge.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .AddEmsServices()
            .AddUiServices();

        return builder.Build();
    }
}

public static class ServiceCollectionExtensions
{
    public static MauiAppBuilder AddEmsServices(this MauiAppBuilder builder)
    {
        builder.Services
            .GetConventionalAssemblies()
            .RegisterConventionalServices()
            .AddPersistence();
        return builder;
    }

    public static MauiAppBuilder AddUiServices(this MauiAppBuilder builder)
    {
        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services
            .AddLocalization(x => x.ResourcesPath = "Resources/Localization")
            .AddMudServices();
        return builder;
    }
}