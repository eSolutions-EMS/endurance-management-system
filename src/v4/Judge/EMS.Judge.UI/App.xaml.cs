using Common.Startup;

namespace EMS.Judge.UI;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App(IEnumerable<IInitializer> initializers)
    {
        InitializeComponent();

        MainPage = new MainPage();

        foreach (var initializer in initializers)
        {
            initializer.Run();
        }
    }
}
