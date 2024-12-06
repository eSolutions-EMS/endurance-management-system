using Microsoft.Extensions.Logging;
using Not.Filesystem;
using Not.Injection;
using NTS.Judge.MAUI.Injection;

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

        var app = builder.Build();


        return app;
    }
}
