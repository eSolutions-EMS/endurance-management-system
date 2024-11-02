using Not.Filesystem;
using Not.Serialization;
using Not.Startup;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Objects;
using NTS.Judge.MAUI.Server;

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
        JudgeMauiServer.Start(serviceProvider);

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("|   Judge Integrated Server started    |");
        Console.WriteLine("----------------------------------------");
    }
}
