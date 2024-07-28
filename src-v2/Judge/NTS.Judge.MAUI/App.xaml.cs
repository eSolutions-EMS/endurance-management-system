using Not.Startup;
// using NTS.Judge.MAUI.Server;

namespace NTS.Judge.MAUI;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App(IEnumerable<IStartupInitializer> initializers, IEnumerable<IStartupInitializerAsync> initializersAsync, IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = new MainPage();

        foreach (var initializer in initializers)
        {
            initializer.RunAtStartup();
        }
        foreach (var initializer in initializersAsync)
        {
            initializer.RunAtStartup();
        }

        StartIntegratedServer(serviceProvider);
    }

    private void StartIntegratedServer(IServiceProvider sericeProvider)
    {
        // JudgeMauiServer.Start(sericeProvider);

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("|   Judge Integrated Server started    |");
        Console.WriteLine("----------------------------------------");
    }
}
