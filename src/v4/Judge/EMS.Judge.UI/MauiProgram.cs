using EMS.Domain.Startup;
using EMS.Judge.UI.Data;
using EMS.Persistence.Configuration;
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
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddPersistanceServices();
        builder.Services.AddDomainServices();
        builder.Services.AddLocalization();

        return builder.Build();
    }
}
