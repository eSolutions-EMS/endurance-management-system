using EnduranceJudge.Application.Services;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views;

public partial class ShellWindow : Window
{
    public ShellWindow(ISettings settings) : this()
    {
        this.Version.Text = settings.Version;
    }
    
    public ShellWindow()
    {
        InitializeComponent();
    }
}
