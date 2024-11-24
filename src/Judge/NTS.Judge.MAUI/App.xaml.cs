using Not.Startup;
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

    void StartIntegratedServer(IServiceProvider serviceProvider)
    {
        JudgeMauiServer.Start(serviceProvider);

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("|   Judge Integrated Server started    |");
        Console.WriteLine("----------------------------------------");
    }
}
