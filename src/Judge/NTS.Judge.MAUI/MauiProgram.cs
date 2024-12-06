using System.Diagnostics;
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

        StartHub();

        return app;
    }

    static void StartHub()
    {
        try
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var info = new ProcessStartInfo
            {
                FileName = Path.Combine(currentDirectory, "NTS.Judge.MAUI.Server.exe"),
            };

            var hubProcess = Process.Start(info);
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                if (hubProcess != null && !hubProcess.HasExited)
                {
                    hubProcess.CloseMainWindow();
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}
