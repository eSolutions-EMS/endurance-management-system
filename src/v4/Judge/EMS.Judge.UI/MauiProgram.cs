using Common.Conventions;
using EMS.Judge.UI.Data;
using EMS.Persistence.Startup;
using Microsoft.Extensions.Logging;

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
            .ReferencePersistence();
        return builder;
    }

    public static MauiAppBuilder AddUiServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<WeatherForecastService>()
            .AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services.AddLocalization(x => x.ResourcesPath = "Resources/Localization");
        return builder;
    }
}