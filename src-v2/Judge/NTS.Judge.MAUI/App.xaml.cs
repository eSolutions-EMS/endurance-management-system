using Not.Filesystem;
using Not.Serialization;
using Not.Startup;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Objects;
using NTS.Judge.MAUI.Server;
using System.Diagnostics;

namespace NTS.Judge.MAUI;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App(IEnumerable<IStartupInitializer> initializers, IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = new MainPage();

        foreach (var initializer in initializers)
        {
            initializer.RunAtStartup();
        }

        StartIntegratedServer(serviceProvider);
    }

    private void StartIntegratedServer(IServiceProvider serviceProvider)
    {
        //var thread = new Thread(async () =>
        //{
        //    await JudgeMauiServer.StartServer(serviceProvider);
        //});
        //thread.Start();

        var info = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run --project \"D:\\Source\\NTS\\not-timing-system\\src-v2\\NTS.Console\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = false,  
        };

        var process = Process.Start(info);

        Trace.WriteLine("----------------------------------------", "console");
        Trace.WriteLine("|   Judge Integrated Server started    |", "console");
        Trace.WriteLine("----------------------------------------", "console");
    }
}
